using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicObject : MonoBehaviour
{
    public GameObject logicSlot;
    public Vector3 startPos;
    public Vector3 targetPos;
    public int evidenceNum;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("LogicSlot"))
        {
            Debug.Log("collided");
            GameObject tmp = collision.gameObject;
            if(tmp.GetComponent<LogicSlot>().logicObject != null)
            {
                tmp.GetComponent<LogicSlot>().logicObject.GetComponent<LogicObject>().logicSlot = null;
                tmp.GetComponent<LogicSlot>().logicObject.GetComponent<LogicObject>().targetPos = tmp.GetComponent<LogicSlot>().logicObject.GetComponent<LogicObject>().startPos;
            }
            logicSlot = collision.gameObject;
            targetPos = collision.transform.position + new Vector3(0, 0, -1);
            logicSlot.GetComponent<LogicSlot>().logicObject = this.gameObject;
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
