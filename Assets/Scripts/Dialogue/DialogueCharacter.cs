using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "New Character")]
public class DialogueCharacter : ScriptableObject
{
    public string charName;
    public Sprite[] sprites;
}
