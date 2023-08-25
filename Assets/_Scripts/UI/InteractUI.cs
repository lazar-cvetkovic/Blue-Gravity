using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractUI : MonoBehaviour
{
    PlayerInputActions _playerInput;
    DialogueTrigger _dialogueScript;
    RectTransform _rectTransform;
    bool _canPress = false;

    private void Awake()
    {
        _canPress = false;
        _playerInput = new();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _rectTransform.localScale = Vector3.zero;
        DialogueTrigger.OnInteract += HandleInteractUI;
        DialogueTrigger.OnCloseInteraction += CloseInteraction;
        _playerInput.Player.Interact.performed += HandleButtonPressing;
    }

    private void OnDestroy()
    {
        DialogueTrigger.OnInteract -= HandleInteractUI;
        DialogueTrigger.OnCloseInteraction -= CloseInteraction;
        _playerInput.Player.Interact.performed -= HandleButtonPressing;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void HandleInteractUI(DialogueTrigger dialogueScript)
    {
        _dialogueScript = dialogueScript;
        _canPress = true;
        Animate(true);
    }

    private void HandleButtonPressing(InputAction.CallbackContext context)
    {
        if (_canPress)
        {
            _canPress = false;
            _dialogueScript.StartDialogue();
            Animate(false);
        }
    }

    private void CloseInteraction() => Animate(false);

    private void Animate(bool isOpening)
    {
        var wantedScaleVector = isOpening ? Vector3.one : Vector3.zero;
        _rectTransform.LeanScale(wantedScaleVector, 0.5f).setEaseInOutExpo();
    }
}
