using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CoinPickUp coin;

    public int currentCoin;

    public void AddCoin(int coin)
    {
        currentCoin += coin;
        UIController.instance.UpdateCoins();
    }

    public void DropCoin(Vector3 position, int value)
    {
        CoinPickUp newCoin = Instantiate(this.coin, position + new Vector3(.2f, .1f, 0f), Quaternion.identity);
        newCoin.coinAmount=value;
        newCoin.gameObject.SetActive(true);
    }

    public void SpendCoin(int coin)
    {
        currentCoin -= coin;
        UIController.instance.UpdateCoins();
    }
}

