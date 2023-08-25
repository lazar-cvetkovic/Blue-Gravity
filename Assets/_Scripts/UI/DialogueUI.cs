using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] RectTransform _dialogueBox;
    [SerializeField] TMP_Text _dialogueText;
    [SerializeField] Image _dialogueImage;
    [SerializeField] CanvasGroup _textCanvasGroup;
    [SerializeField] CanvasGroup _imageCanvasGroup;

    PlayerInputActions _playerInput;
    DialogueSO _dialogueData;

    int _dialogueIndex;
    int _dialogueLength;
    float _startingHeight;

    bool _isDialogueActive;
    bool _canClick = true;
    bool _isShop;

    private void Awake()
    {
        _playerInput = new();
        _dialogueLength = 0;
        _startingHeight = _dialogueBox.sizeDelta.y;
        _dialogueBox.sizeDelta = new Vector2(_dialogueBox.sizeDelta.x, 0);

        _textCanvasGroup.alpha = 0;
        _imageCanvasGroup.alpha = 0;
    }

    private void Start()
    {
        DialogueTrigger.OnDialogue += ActivateDialogue;
        _playerInput.Player.Fire.performed += ContinueDialogue;
    }


    private void OnDestroy()
    {
        DialogueTrigger.OnDialogue -= ActivateDialogue;
        _playerInput.Player.Fire.performed -= ContinueDialogue;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    public void ActivateDialogue(DialogueSO dialogueData, bool isShop)
    {
        if (_isDialogueActive) return;

        _dialogueLength = dialogueData.DialogueSequence.Length;
        _dialogueData = dialogueData;
        _dialogueText.text = "";
        _isDialogueActive = true;
        _isShop = isShop;

        HandleDialogSequence(dialogueData);
        HandleDialogueAnimation(true);
    }

    private void ContinueDialogue(InputAction.CallbackContext obj)
    {
        if (_isDialogueActive && _canClick)
        {
            HandleDialogSequence(_dialogueData);
            _canClick = false;
        }

    }

    private void HandleDialogSequence(DialogueSO dialogueData)
    {
        if (_dialogueIndex >= _dialogueLength)
        {
            DeactivateDialogue();
            return;
        }

        _textCanvasGroup.LeanAlpha(0, 0.5f);
        _imageCanvasGroup.LeanAlpha(0, 0.5f).setOnComplete(() =>
        {
            _dialogueText.text = dialogueData.DialogueSequence[_dialogueIndex];

            var isNpc = _dialogueIndex % 2 == 0;
            var index = GameManager.Instance.SelectedSprite;

            var dialogueSprite = isNpc ? dialogueData.CharacterSprites[0] : PlayerSprites.Instance.Sprites[index];
            _dialogueImage.sprite = dialogueSprite;

            _dialogueIndex++;
            _canClick = true;

            Fade(1, 0);
        });
    }

    private void DeactivateDialogue()
    {
        _isDialogueActive = false;
        _dialogueIndex = 0;

        HandleDialogueAnimation(false);

        if (_isShop) StartCoroutine(StartShopping());
    }

    private IEnumerator StartShopping()
    {
        yield return new WaitForSeconds(1);
        ShopManager.Instance.SetupShop();
    }

    private void HandleDialogueAnimation(bool isActivating)
    {
        float valueFrom = isActivating ? 0 : _startingHeight;
        float valueTo = isActivating ? _startingHeight : 0;
        int fadeValue = isActivating ? 1 : 0;
        var tweenType = isActivating ? LeanTweenType.easeOutQuad : LeanTweenType.easeInQuad;

        LeanTween.value(gameObject, valueFrom, valueTo, 1)
            .setOnUpdate((float value) =>
            {
                Vector2 size = _dialogueBox.sizeDelta;
                size.y = value;
                _dialogueBox.sizeDelta = size;
            })
            .setEase(tweenType)
            .setOnComplete(() => _canClick = true);

        Fade(fadeValue, 0.2f);
    }

    private void Fade(float fadeValue, float fadeDelay)
    {
        _textCanvasGroup.LeanAlpha(fadeValue, 0.5f).delay = fadeDelay;
        _imageCanvasGroup.LeanAlpha(fadeValue, 0.5f).delay = fadeDelay;
    }
}
