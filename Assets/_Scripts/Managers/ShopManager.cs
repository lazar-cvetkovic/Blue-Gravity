using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] RectTransform _shop;
    [SerializeField] CanvasGroup _shopBackgroundCanvasGroup;
    [SerializeField] GameObject[] _buyButtons;
    [SerializeField] TMP_Text[] _currencyTexts;

    Image _background;

    private void Awake()
    {
        base.Awake();
        _background = _shopBackgroundCanvasGroup.gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        _shop.localPosition = new Vector2(0, -Screen.height);
        _shopBackgroundCanvasGroup.alpha = 0;

        var spritesBought = GameManager.Instance.SpritesBought;

        for (int i = 0; i < spritesBought.Length; i++)
        {
            if (i != 0)
            {
                _buyButtons[i - 1].SetActive(!spritesBought[i]);
            }
        }

        SetCurrencyText(Currency.EMERALD);
        SetCurrencyText(Currency.DIAMOND);
        SetCurrencyText(Currency.PLATINUM);
    }

    public void SetupShop() => HandleShopAnimation(true);

    public void CloseShop() => HandleShopAnimation(false);

    private void HandleShopAnimation(bool isActivating)
    {
        float moveValue = isActivating ? 0 : -Screen.height;
        float alphaValue = isActivating ? 0.7f : 0;

        _shop.LeanMoveLocalY(moveValue, 0.5f).setEaseOutExpo();
        _shopBackgroundCanvasGroup.LeanAlpha(alphaValue, 0.5f);
    }

    public void BuyOrangeSet() => Buy(0, 100);

    public void BuyBlueSet() => Buy(1, 300);

    public void BuyPurpleSet() => Buy(2, 1000);

    public void SellEmeralds() => Sell(Currency.EMERALD, 20);

    public void SellDiamonds() => Sell(Currency.DIAMOND, 70);

    public void SellPlatinum() => Sell(Currency.PLATINUM, 200);


    private void Buy(int buttonIndex, int price)
    {
        bool isSuccessful = GameManager.Instance.BuySprite(buttonIndex + 1, price);
        _buyButtons[buttonIndex].SetActive(!isSuccessful);

        if(!isSuccessful) HandleError();
    }

    private void Sell(Currency currencyType, int coins)
    {
        bool isSuccessful = GameManager.Instance.SellCurrency(currencyType, coins);

        if (!isSuccessful) 
            HandleError();
        else
            SetCurrencyText(currencyType);
    }

    private IEnumerator HandleError()
    {
        _background.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        _background.color = Color.white;
    }

    private void SetCurrencyText(Currency currencyType)
    {
        var currencyAmount = GameManager.Instance.Currencies[currencyType];

        int index = currencyType switch
        {
            Currency.EMERALD => 0,
            Currency.DIAMOND => 1,
            Currency.PLATINUM => 2
        };

        _currencyTexts[index].text = $"You have: {currencyAmount}";
    }
}
