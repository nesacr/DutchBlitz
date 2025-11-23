using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private GameObject selectedCardGO;
    private Outline selectedCardOutline;

    private void Awake()
    {
        Instance = this;        
    }

    public void SelectCard(GameObject cardGO)
    {
        selectedCardGO = cardGO;
        selectedCardOutline = cardGO.GetComponentInChildren<Outline>();
        cardGO.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        Debug.Log("Card Selected: " + cardGO.name);
    }

    public void PlacingCard(GameObject cardGO)
    {

        // if card is untagged, tag it as player
        //if (cardGO.tag == "Untagged")
        //{
        //    cardGO.tag = "Player";
        //}
        //TODO: check the parent tag instead of the card tag itself

        Player.Instance.RemoveCardFromPile(cardGO);

        if(selectedCardOutline != null)
        {
            selectedCardOutline.enabled = false;
            selectedCardOutline.transform.localScale = new Vector3(1f, 1f, 1f);
            cardGO.transform.localScale = new Vector3(1f, 1f, 1f);
        }       
        
    }
}
