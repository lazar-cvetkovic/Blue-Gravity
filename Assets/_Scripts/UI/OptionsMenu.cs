using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static event Action OptionsMenuClosed;

    [SerializeField] Transform _optionsTransform;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Image _musicButtonImage;
    [SerializeField] Image _sfxButtonImage;
    [SerializeField] AudioSpritesSO _audioSprites;

    CanvasGroup _background;
    bool _isMusicMuted;
    bool _isSfxMuted;

    private void Awake()
    {
        _background = GetComponentInChildren<CanvasGroup>();
    }

    private void OnEnable()
    {
        _musicSlider.value = AudioManager.Instance.MusicVolume;
        _sfxSlider.value = AudioManager.Instance.SfxVolume;

        _background.alpha = 0;
        _background.LeanAlpha(0.7f, 0.5f);

        _optionsTransform.localPosition = new Vector2(0, -Screen.height);
        _optionsTransform.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseOptions();
    }

    public void CloseOptions()
    {
        _background.LeanAlpha(0, 0.5f);
        _optionsTransform.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnAnimationComplete);
        OptionsMenuClosed?.Invoke();
    }

    private void OnAnimationComplete() => gameObject.SetActive(false);

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        ChangeMusicSprite();
        _isMusicMuted = !_isMusicMuted;
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        ChangeSfxSprite();
        _isSfxMuted = !_isSfxMuted;
    }

    public void ChangeMusicVolume()
    {
        AudioManager.Instance.ChangeMusicVolume(_musicSlider.value);
        if (_musicSlider.value == 0 || _isMusicMuted)
            ToggleMusic();
    }

    public void ChangeSFXVolume()
    {
        AudioManager.Instance.ChangeSFXVolume(_sfxSlider.value);
        if (_sfxSlider.value == 0 || _isSfxMuted)
            ToggleSFX();
    }

    void ChangeMusicSprite() => _musicButtonImage.sprite = _musicButtonImage.sprite == _audioSprites.MusicOn ? _audioSprites.MusicOff : _audioSprites.MusicOn;

    void ChangeSfxSprite() => _sfxButtonImage.sprite = _sfxButtonImage.sprite == _audioSprites.SfxOn ? _audioSprites.SfxOff : _audioSprites.SfxOn;

}
