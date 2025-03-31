using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] LayerMask target;
    [SerializeField] int goldValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1<<collision.gameObject.layer) & target) != 0)
        {
            GameManager.Instance.PlayerManager.player.Currency.GoldAdd(CurrenyType.Gold, goldValue);
            Destroy(gameObject);
        }       
    }
}
