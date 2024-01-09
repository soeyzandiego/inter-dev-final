using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogicObject : MonoBehaviour
{
    [HideInInspector] public LogicSlot logicSlot;
    [HideInInspector] public LogicSlot touchingSlot;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public int evidenceNum;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("LogicSlot"))
        {
            //LogicSlot slot = collision.gameObject.GetComponent<LogicSlot>();
            //if(slot.logicObject != null)
            //{
            //    //Debug.Log("swapped logic objects");
            //    LogicObject curObject = slot.logicObject;
            //    curObject.SetTarget(curObject.startPos, true);
                
            //    slot.logicObject.logicSlot = null;
            //}
            touchingSlot = collision.GetComponent<LogicSlot>();
            targetPos = collision.transform.position + new Vector3(0, 0, -1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (logicSlot != null)
        {
            logicSlot.GetComponent<LogicSlot>().logicObject = null;
        }
        logicSlot = null;
        touchingSlot = null;
        targetPos = startPos;
    }

    public void SetTarget(Vector3 newPos, bool moveTo)
    {
        targetPos = newPos;

        if (moveTo) { transform.position = targetPos; }
    }

    public void MoveToTarget()
    {
        transform.position = targetPos;

        if (touchingSlot != null)
        {
            logicSlot = touchingSlot;
            if (logicSlot.logicObject != null)
            {
                // swap logic objects
                LogicObject curObject = logicSlot.logicObject;
                curObject.logicSlot = null;
                curObject.SetTarget(curObject.startPos, true);
            }
            logicSlot.logicObject = this;
            logicSlot.currentEvidence = evidenceNum;
        }
    }
}
