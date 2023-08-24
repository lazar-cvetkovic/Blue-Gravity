using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string[] DialogueSequence;
    public Sprite[] CharacterSprites;
}
