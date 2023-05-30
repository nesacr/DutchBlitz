using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Text valueText;
    public Image genderImage;
    public Color colorDisplay;

    // Start is called before the first frame update
    void Start()
    {
        genderImage.sprite = card.gender;
        valueText.text = card.cardValue.ToString();
        colorDisplay = card.colorDisplay;

    }

}
