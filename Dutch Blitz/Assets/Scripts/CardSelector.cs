using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardSelector : MonoBehaviour
{
    public GameObject cardColor;
    public Outline outline;
    public CardDisplay cardDisplay;

    void Start()
    {
        outline = cardColor.GetComponent<Outline>();
        outline.enabled = false;
       
        cardDisplay = GetComponent<CardDisplay>();

        if(cardDisplay == null)
        {
            Debug.LogError("CardDisplay component not found on " + gameObject.name);
        }

    }

   public void highlightCard()
    {
       // Implementation for highlighting the card
       //Debug.Log("Card highlighted: " + gameObject.name);
       outline.enabled = true;
    }

    public void unHighlightCard()
    {
       // Implementation for un-highlighting the card
       //Debug.Log("Card un-highlighted: " + gameObject.name);
       outline.enabled = false;
    }

    public void OnMouseClick()
    {
        outline.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        PlayerManager.Instance.SelectCard(gameObject);
        if(IsStartingCard(cardDisplay))
        {
            PileManager.Instance.AutoPlaceStartingCard(cardDisplay);

            PlayerManager.Instance.PlacingCard(gameObject);
        }

        outline.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public bool IsStartingCard(CardDisplay card)
    {
        if (card.valueText.text == "1")
        {
            return true;
        }
        return false;
    }
}
