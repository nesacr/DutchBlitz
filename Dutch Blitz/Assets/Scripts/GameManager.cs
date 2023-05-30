using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //TODO: need to create stack for wood pile and for blitz pile


    public List<CardDisplay> deck = new List<CardDisplay>();
    public Transform[] cardSlots;
    public Transform woodSlot;
    public Transform blitzSlot;

    public bool[] availableCardSlots;

    public void DealCards()
    {
        if(deck.Count >= 1)
        {
            CardDisplay randCard = deck[Random.Range(0, deck.Count)];

            for(int i = 0; i < availableCardSlots.Length; i++)
            {
                if(availableCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.transform.position = cardSlots[i].position;
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    return;
                }
            }
        }
    }

}
