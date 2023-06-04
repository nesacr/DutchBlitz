//#define DEBUGGING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Player : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
       
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
        for(int i = 0; i < inputDeck.Count - 1; i++)
        {
            T temp = inputDeck[i];
            int rand = Random.Range(i, inputDeck.Count);
            inputDeck[i] = inputDeck[rand];
            inputDeck[rand] = temp;
        }
    }

   
    void DealCards()
    {
        //Stack 1 - deal 10 cards to blitz pile
        for(int i = 0; i < 10; i++)
        {
            //add card from front of deck to blitz pile
            stacks[blitzGO].Push(playerDeck[0]);

            //remove card from deck
            playerDeck.RemoveAt(0);
        }

        stacks[slotOneGO].Push(playerDeck[0]);
        playerDeck.RemoveAt(0);
        stacks[slotTwoGO].Push(playerDeck[0]);
        playerDeck.RemoveAt(0);
        stacks[slotThreeGO].Push(playerDeck[0]);
        playerDeck.RemoveAt(0);

        // Stack 4 - deal remainder of deck to wood pile stack
        while (playerDeck.Count > 0)
        {
            //playerStacks[3].Push(playerDeck[0]);
            stacks[woodGO].Push(playerDeck[0]);
            playerDeck.RemoveAt(0);
        }
    }

    void ChangeCardDisplay(GameObject cardGO)
    {
        //cardGO.GetComponent<CardDisplay>().card;
        // make sure card has something to disply if stack isn't empty
        if(stacks[cardGO].TryPeek(out Card currentCard))
        {
            Debug.Log($"Changing card display for: {cardGO} to {currentCard}");
            cardGO.GetComponent<CardDisplay>().card = currentCard;
            cardGO.GetComponent<CardDisplay>().RefreshCardDisplay();
        }

        else
        {
            cardGO.SetActive(false);
        }
    }

    void UpdateAllCardDisplays()
    {
        foreach(var key in stacks.Keys)
        {
            //Debug.Log($"Changing card display for: {key} to {cardStacks[key].Peek()}");
            ChangeCardDisplay(key);
        }
    }

    //helper function for assigning game objects
    private GameObject FindChildGameObjectByName(string gameObjectName)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).name == gameObjectName)
            {
                return transform.GetChild(i).gameObject;
            }
        }

        return null;
    }


#if DEBUGGING
    [CustomEditor(typeof(Player))]
    public class StackPreview : Editor
    {
        public override void OnInspectorGUI()
        {

            // get the target script as TestScript and get the stack from it
            var player = (Player)target;
            var blitz = player.blitzPile;
            var slotOne = player.slotOnePile;
            var slotTwo = player.slotTwoPile;
            var slotThree = player.slotThreePile;
            var wood = player.woodPile;

            // some styling for the header, this is optional
            var bold = new GUIStyle();
            bold.fontStyle = FontStyle.Bold;
            GUILayout.Label("Blitz Stack", bold);

            // add a label for each item, you can add more properties
            // you can even access components inside each item and display them
            // for example if every item had a sprite we could easily show it 
            foreach (var item in blitz)
            {
                GUILayout.Label(item.name);
            }

            GUILayout.Label("Slot One", bold);
            foreach (var item in slotOne)
            {
                GUILayout.Label(item.name);
            }

            GUILayout.Label("Slot Two", bold);
            foreach (var item in slotTwo)
            {
                GUILayout.Label(item.name);
            }

            GUILayout.Label("Slot Three", bold);
            foreach (var item in slotThree)
            {
                GUILayout.Label(item.name);
            }

            GUILayout.Label("Wood", bold);
            foreach (var item in wood)
            {
                GUILayout.Label(item.name);
            }
        }
    }
#endif
}
