using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public Vector3 selectedScaleAmount = new Vector3(1.1f, 1.1f, 1f);

    private GameObject selectedCardGO;
    private Outline selectedCardOutline;
    private Vector3 originalCardScale;
    public bool bCardSelected = false;

    private void Awake()
    {
        Instance = this;        
    }

    public void SelectCard(GameObject cardGO)
    {
        selectedCardGO = cardGO;
        selectedCardOutline = cardGO.GetComponentInChildren<Outline>();
        originalCardScale = selectedCardGO.transform.localScale;
        selectedCardGO.transform.localScale = selectedScaleAmount;
        bCardSelected = true;
    }

    public void PlacingCard(GameObject cardGO)
    {
        Player.Instance.RemoveCardFromPile(cardGO);

        DeselectCard(cardGO);
    }
    public void DeselectCard(GameObject cardGO = null)
    {
        GameObject go = cardGO ?? selectedCardGO;

        if (selectedCardOutline != null)
        {

            if (cardGO)
            {
                cardGO.transform.localScale = originalCardScale;
            }
            else if(go)
            {
                go.transform.localScale = originalCardScale;
            }
            selectedCardOutline.enabled = false;
            selectedCardOutline.transform.localScale = originalCardScale;
        }

        var cardColor = go.GetComponent<CardSelector>();
        if (cardColor != null && cardColor.cardColor != null)
        {
            cardColor.cardColor.transform.localScale = originalCardScale;
        }

        bCardSelected = false;
        selectedCardGO = null;
        selectedCardOutline = null;
    }
}

