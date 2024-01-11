using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagGlass : MonoBehaviour
{
    static Animator anim;
    static Transform staticTransform;

    static string objectName;
    static ObjectClick.SpawnBox onRipple;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        staticTransform = GetComponent<Transform>();
    }

    static public void ActivateGlass(ObjectClick.SpawnBox _onRipple, string _objectName, Vector3 pos)
    {
        anim.SetTrigger("Ripple");
        onRipple = _onRipple;
        objectName = _objectName;

        staticTransform.position = pos;
    }

    // used by animation event
    public void PlayOnRipple()
    {
        onRipple(objectName);
    }

}
