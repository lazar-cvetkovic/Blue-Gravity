using System;
using UnityEngine;

public class PlayerSprites : Singleton<PlayerSprites>
{
    public Sprite[] Sprites;
    [SerializeField] RuntimeAnimatorController[] _animatorControllers;

    SpriteRenderer _currentPlayerSprite;
    Animator _animator;

    private void Awake()
    {
        base.Awake();
        _currentPlayerSprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ChangeSprite();
        GameManager.OnSpriteChange += ChangeSprite;
    }

    private void OnDestroy()
    {
        GameManager.OnSpriteChange -= ChangeSprite;
    }

    private void ChangeSprite()
    {
        var spriteIndex = GameManager.Instance.SelectedSprite;
        _currentPlayerSprite.sprite = Sprites[spriteIndex];
        _animator.runtimeAnimatorController = _animatorControllers[spriteIndex];
    }
}
