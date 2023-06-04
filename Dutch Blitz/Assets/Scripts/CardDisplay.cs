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

    // Start is called before the first frame update
    void Start()
    {
        RefreshCardDisplay();
    }

    public void RefreshCardDisplay()
    {
        genderImage.sprite = card.genderDisplay;
        valueText.text = card.cardValue.ToString();
        colorDisplay.color = card.colorDisplay;
    }

}
