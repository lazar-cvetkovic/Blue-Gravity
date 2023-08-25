using EasyTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnterGameScene : MonoBehaviour, IInteractable
{
    public static event Action OnInteractionStart;
    public static event Action OnInteractionEnd;

    [SerializeField] bool _isEnteringShop;

    PlayerInputActions _playerInput;

    bool _canInteract = false;

    private void Awake()
    {
        _playerInput = new();
    }

    private void Start()
    {
        _playerInput.Player.Interact.performed += Load;
    }

    private void OnDestroy()
    {
        _playerInput.Player.Interact.performed -= Load;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Load(InputAction.CallbackContext obj)
    {
        if (!_canInteract) return;

        int sceneIndex = _isEnteringShop ? 1 : 2;
        LevelManager.Instance.LoadScene(sceneIndex);
    }

    public void CloseInteraction()
    {
        _canInteract = false;
        OnInteractionEnd?.Invoke();
    }

    public void Interact()
    {
        _canInteract = true;
        OnInteractionStart?.Invoke();
    }

    
}
