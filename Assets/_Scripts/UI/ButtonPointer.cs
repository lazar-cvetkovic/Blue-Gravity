using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonPointer : MonoBehaviour
{
    public static Action<float> ClickedEnter;

    ButtonSettings[] _buttons;
    Dictionary<int, float> _buttonYPositions = new();

    int _selectedButtonIndex;
    bool _arrowInputHandled = true;

    private void Awake() => _buttons = transform.parent.GetComponentsInChildren<ButtonSettings>();

    private void Start()
    {
        StartCoroutine(AddButtonPositionsCoroutine());
        ButtonSettings.PointerChanged += ChangePointerPosition;
        OptionsMenu.OptionsMenuClosed += ResetPointer;
    }

    private void OnDestroy()
    {
        ButtonSettings.PointerChanged -= ChangePointerPosition;
        OptionsMenu.OptionsMenuClosed -= ResetPointer;
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

    private void Update()
    {
        HandleEnterInput();
        HandleArrowInputs();
    }

    private void HandleEnterInput()
    {
        if(Input.GetKeyDown(KeyCode.Return))
            ClickedEnter?.Invoke(GetComponent<RectTransform>().localPosition.y);
    }

    private void HandleArrowInputs()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(verticalInput) <= 0.1f)
        {
            _arrowInputHandled = true;
            return;
        }

        if (_arrowInputHandled)
        {
            _arrowInputHandled = false;
            AudioManager.Instance.PlaySFX(SoundType.BUTTON_HOVER);

            int direction = (int)Mathf.Sign(verticalInput);

            _selectedButtonIndex = (_selectedButtonIndex + _buttons.Length - direction) % _buttons.Length;
            ChangePointerPosition(_buttonYPositions[_selectedButtonIndex]);
        }
    }
}
