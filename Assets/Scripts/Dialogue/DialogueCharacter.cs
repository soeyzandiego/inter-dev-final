using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "New Character")]
public class DialogueCharacter : ScriptableObject
{
    [SerializeField] public string charName;
    [SerializeField] public Sprite[] sprites;
}
