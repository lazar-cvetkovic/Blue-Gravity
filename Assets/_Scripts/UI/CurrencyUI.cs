using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] TMP_Text _coins;
    [SerializeField] TMP_Text _emerald;
    [SerializeField] TMP_Text _diamond;
    [SerializeField] TMP_Text _platinum;

    private void Start()
    {
        GameManager.OnCoinsChanged += UpdateCurrencies;
        UpdateCurrencies();
    }

    private void OnDestroy()
    {
        GameManager.OnCoinsChanged -= UpdateCurrencies;
    }

    private void UpdateCurrencies()
    {
        _coins.text = GameManager.Instance.Coins.ToString();
        _emerald.text = GameManager.Instance.Currencies[Currency.EMERALD].ToString();
        _diamond.text = GameManager.Instance.Currencies[Currency.DIAMOND].ToString();
        _platinum.text = GameManager.Instance.Currencies[Currency.PLATINUM].ToString();
    }
}
