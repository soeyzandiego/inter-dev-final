using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventFlag : MonoBehaviour
{
    public abstract string ClueID { get; set; }
    public abstract void Event();
}
