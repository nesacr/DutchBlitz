using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseClick()
    {
        if(PlayerManager.Instance.bCardSelected)
        {
            // Deselect the currently selected card
            PlayerManager.Instance.DeselectCard();
        }
    }
}
