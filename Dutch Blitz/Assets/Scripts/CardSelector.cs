using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    public GameObject cardColor;
    public Outline outline;

    void Start()
    {
        outline = cardColor.GetComponent<Outline>();
        outline.enabled = false;
    }

   public void highlightCard()
    {
       // Implementation for highlighting the card
       Debug.Log("Card highlighted: " + gameObject.name);
       outline.enabled = true;
    }

    public void unHighlightCard()
    {
       // Implementation for un-highlighting the card
       Debug.Log("Card un-highlighted: " + gameObject.name);
       outline.enabled = false;
    }
}
