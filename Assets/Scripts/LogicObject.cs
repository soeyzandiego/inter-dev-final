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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("LogicSlot"))
        {
            GameObject tmp = collision.gameObject;
            if(tmp.GetComponent<LogicSlot>().logicObject != null)
            {
                tmp.GetComponent<LogicSlot>().logicObject.GetComponent<LogicObject>().logicSlot = null;
                tmp.GetComponent<LogicSlot>().logicObject.GetComponent<LogicObject>().targetPos = tmp.GetComponent<LogicSlot>().logicObject.GetComponent<LogicObject>().startPos;
            }
            logicSlot = collision.gameObject;
            targetPos = collision.transform.position;
            logicSlot.GetComponent<LogicSlot>().logicObject = this.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        logicSlot = null;
        targetPos = startPos;
        logicSlot.GetComponent<LogicSlot>().logicObject = null;
    }
}
