using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Clickable : MonoBehaviour
{

    // did not need to be abstract, thought would need different functionalities for enabling/disabling diff types of clickables
    // ended up not needing this for ObjectClick since it doesn't look for the collision of a specific object (looks for tags)
    public bool HoveringThis()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(mousePosition, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == gameObject) { return true; }
            else { return false; }
        }
        else
        {
            return false;
        }
    }

    public void SetClickable(bool _clickable)
    {
        if (_clickable) { GetComponent<Collider2D>().enabled = true; }
        else { GetComponent<Collider2D>().enabled = false; }
    }
}
