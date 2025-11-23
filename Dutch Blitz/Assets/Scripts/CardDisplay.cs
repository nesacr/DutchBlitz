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

    public GameObject artworkGO;

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
        // Ensure color database is available if called outside Start
        if (colorDatabase == null)
            colorDatabase = Resources.Load<ColourDatabase>("ColorDB");

        // If there's no card, clear visuals so an empty pile doesn't show stale data
        if (card == null)
        {
            if (valueText != null)
            {
                valueText.text = "";
                valueText.enabled = false;
            }

            if (genderImage != null)
            {
                genderImage.sprite = null;
                genderImage.enabled = false;
            }

            if (colorDisplay != null)
            {
                // clear color and sprite and disable the image so nothing renders
                colorDisplay.color = Color.clear;
                colorDisplay.sprite = null;
                colorDisplay.enabled = false;
            }

            return;
        }

        // ensure UI elements are visible for a real card
        if (valueText != null)
        {
            valueText.enabled = true;
            valueText.text = card.cardValue.ToString();
        }

        if (genderImage != null)
        {
            genderImage.enabled = true;
            genderImage.sprite = card.genderDisplay;
        }

        if (colorDisplay != null && colorDatabase != null)
        {
            colorDisplay.enabled = true;
            colorDisplay.sprite = colorDisplay.sprite; // preserve existing sprite if any
            colorDisplay.color = colorDatabase.GetColor(card.colour);
        }
    }
}
