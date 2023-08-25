using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : SingletonPersistent<GameManager>
{
    public static event Action OnCoinsChanged;
    public static event Action OnGameEnd;
    public static event Action OnSpriteChange;

    public bool[] SpritesBought = { true, false, false, false };
    public int FashionScore;
    public int SelectedSprite;
    public int Coins;

    public Dictionary<Currency, int> Currencies = new()
    { 
        {Currency.EMERALD, 0 },
        {Currency.DIAMOND, 0 },
        {Currency.PLATINUM, 0 }
    };

    public bool BuySprite(int spriteIndex, int spritePrice)
    {
        if (Coins < spritePrice) return false;

        Coins -= spritePrice;
        SpritesBought[spriteIndex] = true;
        SelectedSprite = spriteIndex;

        switch (spriteIndex)
        {
            case 1:
                FashionScore += 1000;
                break;
            case 2:
                FashionScore += 3000;
                break;
            case 3:
                FashionScore += 6000;
                break;
        }

        OnCoinsChanged?.Invoke();
        OnSpriteChange?.Invoke();

        if (FashionScore == 10000) 
            OnGameEnd?.Invoke();

        return true;
    }

    public bool SellCurrency(Currency currencyType, int coins)
    {
        if (Currencies[currencyType] < 1) return false;

        Currencies[currencyType]--;
        Coins += coins;

        OnCoinsChanged?.Invoke();

        return true;
    }

    public void Test()
    {
        Currencies[Currency.EMERALD] = 10;
        Currencies[Currency.DIAMOND] = 10;
        Currencies[Currency.PLATINUM] = 10;
    }
}

public enum Currency
{
    EMERALD,
    DIAMOND,
    PLATINUM
}
