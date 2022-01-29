using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable {

    protected Sprite CoinDestroy;
    protected int CoinAmount = 1;
    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            
            GetComponent<SpriteRenderer>().sprite = CoinDestroy;
            GameManager.instance.coins += CoinAmount;
            GameManager.instance.ShowText("+ " + CoinAmount + " Coin", 35, Color.yellow, transform.position, transform.up * 25, 1.50f);
            Destroy(gameObject);
            
        }
    }

}
