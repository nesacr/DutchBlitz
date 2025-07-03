using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{

    public Card card;
    public Text valueText;
    public Image genderImage;
    public Image colorDisplay;

    private ColourDatabase colorDatabase;

    // Start is called before the first frame update
    void Start()
    {
        // load color database once
        colorDatabase = Resources.Load<ColourDatabase>("ColorDB");
        if(colorDatabase == null )
        {
            Debug.LogError("ColorDB asset not found in Resources folder.");
            return;
        }

        RefreshCardDisplay();
    }

    public void RefreshCardDisplay()
    {
        if (card == null || colorDatabase == null) return;

        // Get color from database using enum
        colorDisplay.color = colorDatabase.GetColor(card.colour); 

        // Apply visuals
        genderImage.sprite = card.genderDisplay;
        valueText.text = card.cardValue.ToString();
    }
}
