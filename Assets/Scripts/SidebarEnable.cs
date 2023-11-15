using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidebarEnable : MonoBehaviour
{
    public GameObject sidebarButton;
    private void OnDisable()
    {
        if(!gameObject.activeSelf)
        {
            sidebarButton.SetActive(true);
        }
    }
}
