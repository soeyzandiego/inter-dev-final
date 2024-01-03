using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicSlot : MonoBehaviour
{
    public LogicObject logicObject;
    public int currentEvidence = 9;
    public int rightEvidence;

    public bool CheckEvidence()
    {
        Debug.Log("checked noun");
        if (logicObject == null)
        {
            return false;
        }
        currentEvidence = logicObject.evidenceNum;
        if (currentEvidence == rightEvidence)
        {
            Debug.Log("correct noun");
            return true;
        }
        return false;
    }
}

