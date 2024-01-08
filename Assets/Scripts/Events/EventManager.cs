using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [HideInInspector] public static EventManager instance;
    [HideInInspector] public delegate void PlayEvent();
    //static Dictionary<string, PlayEvent> flags = new Dictionary<string, PlayEvent>();
    static EventFlag[] flags;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        flags = FindObjectsOfType<EventFlag>();
    }

    public static void CheckFlag(string _id)
    {
        if (flags == null) { return; }
        foreach (EventFlag flag in flags)
        {
            if (flag.ClueID == _id)
            {
                flag.Event();
            }
        }
    }
}
