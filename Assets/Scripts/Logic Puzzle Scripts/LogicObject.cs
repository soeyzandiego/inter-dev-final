using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogicObject : MonoBehaviour
{
    [HideInInspector] public GameObject logicSlot;
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
            LogicSlot slot = collision.gameObject.GetComponent<LogicSlot>();
            if(slot.logicObject != null)
            {
                //Debug.Log("swapped logic objects");
                LogicObject curObject = slot.logicObject;
                curObject.targetPos = curObject.startPos;
                curObject.transform.position = curObject.targetPos;
                
                slot.logicObject.logicSlot = null;
            }
            logicSlot = collision.gameObject;
            targetPos = collision.transform.position + new Vector3(0, 0, -1);
            logicSlot.GetComponent<LogicSlot>().logicObject = this;
            logicSlot.GetComponent<LogicSlot>().currentEvidence = evidenceNum;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (logicSlot != null)
        {
            logicSlot.GetComponent<LogicSlot>().logicObject = null;
        }
        logicSlot = null;
        targetPos = startPos;
    }
}
