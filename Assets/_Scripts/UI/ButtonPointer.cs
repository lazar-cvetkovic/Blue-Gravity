using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ButtonPointer : MonoBehaviour
{
    public static Action<float> ClickedEnter;

    PlayerInputActions _playerInput;
    ButtonSettings[] _buttons;
    Dictionary<int, float> _buttonYPositions = new();
    Vector2 _movementInput;
    WaitForSeconds _waitForSeconds;

    int _selectedButtonIndex;
    bool _arrowInputHandled = true;

    private void Awake()
    {
        _buttons = transform.parent.GetComponentsInChildren<ButtonSettings>();
        _waitForSeconds = new WaitForSeconds(0.2f);
        _playerInput = new();
    }

    private void Start()
    {
        StartCoroutine(AddButtonPositionsCoroutine());
        _playerInput.Player.Enter.performed += HandleEnterInput;
        ButtonSettings.PointerChanged += ChangePointerPosition;
        OptionsMenu.OptionsMenuClosed += ResetPointer;
    }


    private void OnDestroy()
    {
        _playerInput.Player.Enter.performed -= HandleEnterInput;
        ButtonSettings.PointerChanged -= ChangePointerPosition;
        OptionsMenu.OptionsMenuClosed -= ResetPointer;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        HandleArrowInputs();
    }

    private IEnumerator AddButtonPositionsCoroutine()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttonYPositions.Add(i, _buttons[i].gameObject.GetComponent<RectTransform>().localPosition.y);
        }

        ResetPointer();
    }

    private void ChangePointerPosition(float yPosition)
    {
        var rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(rect.localPosition.x, yPosition, rect.localPosition.z);

        _selectedButtonIndex = _buttonYPositions.FirstOrDefault(x => x.Value == yPosition).Key;

        var selectedButton = _buttons[_selectedButtonIndex];
        selectedButton.ChangeButtonImage(selectedButton.ButtonSprites.SelectedSprite, true);
        selectedButton.IsPointerOnTheButton = true;

        _buttons.Where((_, index) => index != _selectedButtonIndex).ToList()
                .ForEach(button => {
                    button.ChangeButtonImage(button.ButtonSprites.NormalSprite, false);
                    button.IsPointerOnTheButton = false;
                });
    }

    private void ResetPointer() => ChangePointerPosition(_buttonYPositions[0]);

    private void HandleEnterInput(InputAction.CallbackContext context)
    {
        ClickedEnter?.Invoke(GetComponent<RectTransform>().localPosition.y);
    }

    private void HandleArrowInputs()
    {
        if(_movementInput.y == 0) return;

        float verticalInput = _movementInput.y;

        if (_arrowInputHandled)
        {
            _arrowInputHandled = false;
            StartCoroutine(InputTimer());

            AudioManager.Instance.PlaySFX(SoundType.BUTTON_HOVER);

            int direction = (int)Mathf.Sign(verticalInput);

            _selectedButtonIndex = (_selectedButtonIndex + _buttons.Length - direction) % _buttons.Length;
            ChangePointerPosition(_buttonYPositions[_selectedButtonIndex]);
        }
    }

    private IEnumerator InputTimer()
    {
        yield return _waitForSeconds;
        _arrowInputHandled = true;
    }

    private void OnMove(InputValue movementValue) => _movementInput = movementValue.Get<Vector2>();

}
