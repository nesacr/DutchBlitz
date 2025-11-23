using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PileManager : MonoBehaviour
{
    public static PileManager Instance;

    public Dictionary<GameObject, bool> centerPiles = new Dictionary<GameObject, bool>();
    
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Debug.Log($"Child found: {child.name}");
            centerPiles.Add(child.gameObject, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AutoPlaceStartingCard(CardDisplay card)
    {
        Debug.Log("Auto placing starting card: " + card.card);
        if(card == null)
        {
            Debug.LogError("AutoPlaceStartingCard called with null card");
            return;
        }

        // set temp values
        GameObject closestPile = null;
        float closestDistance = float.MaxValue;
        Vector3 cardPos = card.transform.position;

        // check for closest transform in free centerPiles
        foreach (var pile in centerPiles)
        {
            Debug.Log($"Pile: {pile.Key.name}, occupied: {pile.Value}");
            if (!pile.Value) // if pile is free
            {
                float distance = Vector3.Distance(cardPos, pile.Key.transform.position);
                Debug.Log($"Distance to pile {pile.Key.name}: {distance}");
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPile = pile.Key;
                }
            }
        }

        if (closestPile != null)
        {
            Debug.Log($"Placing card {card.card} on pile {closestPile.name}");
            // place card on closest pile
            card.transform.position = closestPile.transform.position + new Vector3(0, 0.1f, 0); // slight offset in y to avoid z-fighting
            centerPiles[closestPile] = true; // mark pile as occupied
        }
        else
        {
            Debug.LogWarning("No available piles to place the card.");
        }
    }
}
