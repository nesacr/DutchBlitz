using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Deck deck;
    public List<Card> playerDeck;
    public List<Stack> playerStacks = new List<Stack>(5);
    public Stack<Card> blitzPile;
    public Stack slotOne;
    public Stack slotTwo;
    public Stack slotThree;
    public Stack woodPile;
    //public GameObject woodPile;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerDeck = new List<Card>(deck.deck);
        blitzPile = new Stack<Card>();
        ShuffleDeck<Card>(playerDeck);
        DealCards();
    }

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
        //deal 10 cards to blitz pile first
        for(int i = 0; i < 10; i++)
        {          
            //add card from front of deck to blitz pile
            blitzPile.Push(playerDeck[0]);

            //remove card from deck
            playerDeck.RemoveAt(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
