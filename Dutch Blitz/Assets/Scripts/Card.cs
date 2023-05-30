using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public int cardValue;
    public string colorData;

    public Sprite gender;
    public Color colorDisplay;
    
   
}
