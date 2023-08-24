using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ButtonSettings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static event Action<float> PointerChanged;
    [field: SerializeField] public ButtonSpritesSO ButtonSprites { get; private set; }
    public bool IsPointerOnTheButton;

    [SerializeField] ButtonType _buttonType;
    [SerializeField] int _sceneIndex;

    Button _button;
    RectTransform _rect;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _rect = _button.GetComponent<RectTransform>();
        _button.image.sprite = ButtonSprites.NormalSprite;
    }

    private void Start()
    {
        ButtonPointer.ClickedEnter += HandleEnterClicked;

        switch(_buttonType)
        {
            case ButtonType.LOAD:
                _button.onClick.AddListener(() => LevelManager.Instance.LoadScene(_sceneIndex));
                break;
            case ButtonType.RESTART:
                _button.onClick.AddListener(LevelManager.Instance.RestartScene);
                break;
            case ButtonType.QUIT:
                _button.onClick.AddListener(LevelManager.Instance.QuitGame);
                break;
        }
    }

    private void OnDestroy() => ButtonPointer.ClickedEnter -= HandleEnterClicked;

    private void HandleButtonPressing()
    {
        ChangeButtonImage(ButtonSprites.PressedSprite, true);
        PlaySound(SoundType.BUTTON_CLICK_1);
        StartCoroutine(ChangeToNormalSprite());
    }

    private IEnumerator ChangeToNormalSprite()
    {
        yield return new WaitForSeconds(0.3f);
        ChangeButtonImage(ButtonSprites.SelectedSprite, true);
    }

    public void HandleEnterClicked(float yPointerPosition)
    {
        if (yPointerPosition == _rect.localPosition.y)
        {
            HandleButtonPressing();
            _button.onClick.Invoke();
        }
    }

#region InterfaceMethods

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound(SoundType.BUTTON_HOVER);
        ChangeButtonImage(ButtonSprites.SelectedSprite, true);
        PointerChanged?.Invoke(_rect.localPosition.y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsPointerOnTheButton)
            ChangeButtonImage(ButtonSprites.NormalSprite, false);
    }

    public void OnPointerDown(PointerEventData eventData) => HandleButtonPressing();

#endregion

    private void PlaySound(SoundType soundType) => AudioManager.Instance.PlaySFX(soundType);

    public void ChangeButtonImage(Sprite buttonSprite, bool useShader)
    {
        _button.image.sprite = buttonSprite;
        _button.image.material = useShader ? ButtonSprites.SelectedShader : null;
    }
}

public enum ButtonType
{
    LOAD,
    RESTART,
    QUIT,
    OPTIONS,
    OTHER
}
