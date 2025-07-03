using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardColour
{
    Red,
    Green,
    Blue,
    Yellow
}

[CreateAssetMenu(fileName = "CardColourDatabase", menuName = "Cards/Colour Database")]
public class ColourDatabase : ScriptableObject
{
    public Color red;
    public Color blue;
    public Color green;
    public Color yellow;
    public Color white;

    public Color GetColor(CardColour colour)
    {
        return colour switch
        {
            CardColour.Red => red,
            CardColour.Blue => blue,
            CardColour.Green => green,
            CardColour.Yellow => yellow,
            _ => white,
        };
    }
}
