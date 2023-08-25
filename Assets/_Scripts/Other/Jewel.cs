using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jewel : MonoBehaviour
{
    [SerializeField] Currency _type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            GameManager.Instance.AddCurrencies(_type);
            AudioManager.Instance.PlaySFX(SoundType.PICKUP);
            Destroy(gameObject);
        }
    }
}
