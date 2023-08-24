using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public static event Action<DialogueTrigger> OnInteract;
    public static event Action<DialogueSO, bool> OnDialogue;

    [SerializeField] DialogueSO _dialogueData;
    [SerializeField] bool _isShop;

    public void Interact() => OnInteract?.Invoke(this);

    public void StartDialogue() => OnDialogue?.Invoke(_dialogueData, _isShop);
}
