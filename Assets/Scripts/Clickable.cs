using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Clickable : MonoBehaviour
{
    public bool clickable = true;

    // ended up not needing this for ObjectClick since it doesn't look for the collision of a specific object
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

    public abstract void SetClickable(bool _clickable);
}
