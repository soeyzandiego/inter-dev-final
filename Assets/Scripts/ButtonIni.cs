using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIni : MonoBehaviour
{
    public Button[] buttons;

    // Start is called before the first frame update
    // Iterates through each image under the UI canvas and sets the alphaHitTestMinimumThreshold (the part of the image that can be hit with raycast) to 1 (needs to be fully opaque)
    void Start()
    {
        buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            b.gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 1;
        }
    }
}
