using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GatePhysics : MonoBehaviour
{
    public bool puzzleComplete;
    private GameObject selectedItem;
    private Vector3 selectOffset;

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D selectedObject = Physics2D.OverlapPoint(mousePosition);
            if (selectedObject != null && selectedObject.gameObject.tag.Equals("GatePuzzleCircles"))
            {
                selectedItem = selectedObject.transform.gameObject;
                selectOffset = selectedItem.transform.position - mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedItem != null)
        {
            selectedItem = null;
        }

        if (selectedItem != null)
        {
            selectedItem.GetComponent<Rigidbody2D>().AddForce(Vector2.Lerp(selectedItem.transform.position, mousePosition, Time.deltaTime));
            Debug.Log("Moving!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "CompletionHitbox")
        {
            puzzleComplete = true;
        }
    }
}
