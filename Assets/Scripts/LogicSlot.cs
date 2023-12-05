using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicSlot : MonoBehaviour
{
    public GameObject logicObject;
    public int currentEvidence;
    public int rightEvidence;


    public bool checkEvidence()
    {
        if (logicObject == null)
        {
            return false;
        }
        currentEvidence = logicObject.GetComponent<LogicObject>().evidenceNum;
        if (currentEvidence == rightEvidence )
        {
            return true;
        }
        return false;
    }
}

