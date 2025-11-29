//#define DEBUGGING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Player : MonoBehaviour
{
    public static Player Instance;

    public GameObject cardPrefab;
    public Transform blitzPileZone;
    public Transform slotOnePileZone;
    public Transform slotTwoPileZone;
    public Transform slotThreePileZone;
    public Transform woodPileZone;

    public Deck deck;
    public List<Card> playerDeck;
    public Dictionary<GameObject, Stack<Card>> stacks;

    public Stack<Card> blitzPile;
    public Stack<Card> slotOnePile;
    public Stack<Card> slotTwoPile;
    public Stack<Card> slotThreePile;
    public Stack<Card> woodPile;

    public GameObject blitzGO;
    public GameObject slotOneGO;
    public GameObject slotTwoGO;
    public GameObject slotThreeGO;
    public GameObject woodGO;

    public TMPro.TextMeshProUGUI woodPileCountText;
    public TMPro.TextMeshProUGUI blitzPileCountText;

    public float cardOffset = 0.1f;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerDeck = new List<Card>(deck.deck);

        InitStacks();

        ShuffleDeck<Card>(playerDeck);

        DealCards();
        UpdateAllCardDisplays();

#if DEBUGGING
        Debug.Log($"TOP OF BLITZ STACK IS: {blitzPile.Peek().cardValue}");
#endif
    }


    //Initialize all player stacks 
    void InitStacks()
    {
        //init game objects
        blitzGO = FindChildGameObjectByName("blitz");
        slotOneGO = FindChildGameObjectByName("slot_1");
        slotTwoGO = FindChildGameObjectByName("slot_2");
        slotThreeGO = FindChildGameObjectByName("slot_3");
        woodGO = FindChildGameObjectByName("wood");

        stacks = new Dictionary<GameObject, Stack<Card>>(5);

        blitzPile = new Stack<Card>();
        slotOnePile = new Stack<Card>();
        slotTwoPile = new Stack<Card>();
        slotThreePile = new Stack<Card>();
        woodPile = new Stack<Card>();

        stacks[blitzGO] = blitzPile;
        stacks[slotOneGO] = slotOnePile;
        stacks[slotTwoGO] = slotTwoPile;
        stacks[slotThreeGO] = slotThreePile;
        stacks[woodGO] = woodPile;
    }


    //Randomizes deck for dealing
    void ShuffleDeck<T>(List<T> inputDeck)
    {
        for (int i = 0; i < inputDeck.Count - 1; i++)
        {
            T temp = inputDeck[i];
            int rand = Random.Range(i, inputDeck.Count);
            inputDeck[i] = inputDeck[rand];
            inputDeck[rand] = temp;
        }
    }


    void UpdatePileCountTexts()
    {
        if (woodPileCountText)
        {
            woodPileCountText.text = woodPile.Count.ToString();
        }
        if (blitzPileCountText)
        {
            blitzPileCountText.text = blitzPile.Count.ToString();
        }
    }


    void DealCards()
    {
        for (int i = 0; i < 10; i++)
        {
            CreateCardFromPlayerDeck(blitzPile, blitzPileZone, "", cardOffset);
        }

        // deal to Slot Piles
        CreateCardFromPlayerDeck(slotOnePile, slotOnePileZone, "PostPile");
        CreateCardFromPlayerDeck(slotTwoPile, slotTwoPileZone, "PostPile");
        CreateCardFromPlayerDeck(slotThreePile, slotThreePileZone, "PostPile");

        // deal remainder of cards to wood pile stack
        while (playerDeck.Count > 0)
        {
            CreateCardFromPlayerDeck(woodPile, woodPileZone, "", cardOffset);
        }

        UpdatePileCountTexts();
    }


    void CreateCardFromPlayerDeck(Stack<Card> stack, Transform spawnPoint, string tagName = "", float offset = 0.0f)
    {
        // take the top card from player deck
        Card topCard = playerDeck[0];
        playerDeck.RemoveAt(0);

        // Add to pile stack
        stack.Push(topCard);

        // Instantiate prefab at pile anchor
        GameObject cardGO = Instantiate(cardPrefab, spawnPoint);

        // Update display to match the card
        CardDisplay display = cardGO.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.card = topCard;
            display.RefreshCardDisplay();
        }

        cardGO.transform.localPosition = new Vector3(offset * (stack.Count - 1), offset * (stack.Count - 1), 0);

        if(tagName != "")
        {
            cardGO.tag = tagName;
        }
    }

    void ChangeCardDisplay(GameObject cardPile)
    {
        var cardDisplay = cardPile.GetComponent<CardDisplay>();
        if (cardDisplay == null)
        {
            Debug.LogWarning($"ChangeCardDisplay: no CardDisplay on {cardPile.name}");
            return;
        }

        // make sure card has something to display if stack isn't empty
        if (stacks[cardPile].TryPeek(out Card currentCard))
        {
            Debug.Log($"Changing card display for: {cardPile.name} to {currentCard}");
            cardDisplay.card = currentCard;
            cardDisplay.RefreshCardDisplay();
        }
        else
        {
            // Clear the display when the pile is empty to avoid stale visuals
            cardDisplay.card = null;
            cardDisplay.RefreshCardDisplay();
            Debug.Log("Card pile is empty.");
        }
    }

    void UpdateAllCardDisplays()
    {
        foreach (var key in stacks.Keys)
        {
            //Debug.Log($"Changing card display for: {key} to {cardStacks[key].Peek()}");
            ChangeCardDisplay(key);
        }
    }

    public void RemoveCardFromPile(GameObject cardGO)
    {
        var parentGO = cardGO.transform.parent.gameObject;

        if (!stacks.ContainsKey(parentGO))
        {
            Debug.LogError("Stack GameObject not found in stacks dictionary.");
            return;
        }

        Stack<Card> pileStack = stacks[parentGO];
        if (pileStack.Count > 0)
        {
            pileStack.Pop();
            UpdatePileCountTexts();

            // Always refresh the pile anchor display so the visible top matches the logical top
            ChangeCardDisplay(parentGO);

            // If the pile became empty, run post-empty logic (e.g. refill from blitz)
            if (pileStack.Count == 0)
            {
                if(parentGO.tag == ("PostPile") && blitzPile.Count > 0)
                {
                   // auto move top card from blitz to the empty Post pile
                   AutoMoveBlitzToPost(parentGO);
                }
            }
        }
    }

    public void AutoMoveBlitzToPost(GameObject postPileGO)
    {
        if (postPileGO == null || blitzPile == null || blitzPile.Count == 0)
        {
            Debug.LogWarning("Attempting to move cards from blitz pile to post pile, but GO is null or empty.");
            return;
        }

        Card cardToMove = blitzPile.Pop();
        UpdatePileCountTexts();

        GameObject movedGO = null;

        if (blitzGO != null && blitzGO.transform.childCount > 0)
        {
            Transform topChild = blitzGO.transform.GetChild(blitzGO.transform.childCount - 1);
            movedGO = topChild.gameObject;

            var cd = movedGO.GetComponent<CardDisplay>();
            if (cd != null)
            {
                cd.card = cardToMove;
                cd.RefreshCardDisplay();
            }

            // reparent / position card to the post pile
            movedGO.transform.SetParent(postPileGO.transform, worldPositionStays: true);
            movedGO.transform.position = postPileGO.transform.position + new Vector3(0, 0.1f, 0);
        }
        else
        {
            // fallback: instantiate new card GO if something went wrong
            movedGO = Instantiate(cardPrefab, postPileGO.transform);
        }
    }

    public void CycleWoodPile()
    {
        if (woodPile == null || woodPile.Count == 0)
        {
            Debug.LogWarning("Wood pile is empty, cannot cycle.");
            return;
        }

        int count = 3;
        int size = woodPile.Count;

        if (count >= size)
        {
            //TODO: maybe add logic here in the future to reshuffle the wood pile back into the deck
            return; // nothing to cycle
        }

        List<Card> list = new List<Card>(woodPile);
        List<Card> newOrder = new List<Card>(size);
        newOrder.AddRange(list.GetRange(count, list.Count - count)); // remaining top becomes new top
        newOrder.AddRange(list.GetRange(0, count)); // cycled cards go to bottom

        // rebuild woodPile stack to match newOrder
        woodPile.Clear();
        for (int i = newOrder.Count - 1; i >= 0; i--)
        {
            woodPile.Push(newOrder[i]);
        }

        // Update the visual CardDisplay components on child card GameObjects (if any)
        if (woodGO != null)
        {
            // Collect immediate children that have CardDisplay (creation order: bottom -> top)
            var childDisplays = new List<CardDisplay>();
            for (int i = 0; i < woodGO.transform.childCount; i++)
            {
                var child = woodGO.transform.GetChild(i);
                var cd = child.GetComponent<CardDisplay>();
                if (cd != null)
                    childDisplays.Add(cd);
            }

            if (childDisplays.Count == 0)
            {
                // No instantiated card GameObjects to update; just update pile display
                ChangeCardDisplay(woodGO);
                return;
            }

            // childDisplays[0] is oldest (bottom) ... childDisplays[last] is newest (top)
            int visualCount = childDisplays.Count;
            if (visualCount != newOrder.Count)
            {
                Debug.LogWarning($"CycleWoodPile: child visual count ({visualCount}) != logical woodPile count ({newOrder.Count}). Updating minimum of both.");
            }

            int itemsToUpdate = Mathf.Min(visualCount, newOrder.Count);

            // Map newOrder[0] (new top) -> childDisplays[last], newOrder[1] -> childDisplays[last-1], ...
            for (int i = 0; i < itemsToUpdate; i++)
            {
                int childIndex = visualCount - 1 - i; // topmost child index for ith card in newOrder
                var cd = childDisplays[childIndex];
                cd.card = newOrder[i];
                cd.RefreshCardDisplay();
            }

            // Finally update pile anchor display as well
            ChangeCardDisplay(woodGO);
        }
        else
        {
            // fallback: update pile anchor only
            ChangeCardDisplay(woodGO);
        }

        Debug.Log($"Cycled wood pile by {count} cards.");
    }

    //helper function for assigning game objects
    private GameObject FindChildGameObjectByName(string gameObjectName)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == gameObjectName)
            {
                return transform.GetChild(i).gameObject;
            }
        }

        return null;
    }
}

