using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    public List<Card> deck = new List<Card>(40);

    void Start()
    {
       
    }
  
    // shuffle the order of the deck, ready for dealing
  

    // Update is called once per frame
    void Update()
    {
        
    }
}
