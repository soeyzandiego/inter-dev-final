using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GatePhysics : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject selectedItem;
    [SerializeField] Vector3 selectOffset;
    private Vector3 mousePosition;

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D selectedObject = Physics2D.OverlapPoint(mousePosition);
            if (selectedObject != null )
            {
                Debug.Log(selectedObject.transform.name);
            }
            if (selectedObject != null && selectedObject.gameObject.tag.Equals("GatePuzzleCircles"))
            {
                selectedItem = selectedObject.transform.gameObject;
                selectOffset = selectedItem.transform.parent.position - mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedItem != null)
        {
            selectedItem = null;
        }
    }

    private void FixedUpdate()
    {
        if (selectedItem != null)
        {
            Vector3 lerp = (mousePosition + selectOffset) - selectedItem.transform.parent.position;
            selectedItem.GetComponentInParent<Rigidbody2D>().velocity = lerp*2;
            Debug.DrawLine(selectedItem.transform.parent.position, selectedItem.transform.parent.position + lerp);
            Debug.Log("Moving!");
        }
    }
}
