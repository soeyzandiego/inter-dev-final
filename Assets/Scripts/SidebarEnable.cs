using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidebarEnable : MonoBehaviour
{
    [SerializeField] GameObject sidebarButton;
    private void OnDisable()
    {
        if(!gameObject.activeSelf)
        {
            sidebarButton.SetActive(true);
        }
    }
}
