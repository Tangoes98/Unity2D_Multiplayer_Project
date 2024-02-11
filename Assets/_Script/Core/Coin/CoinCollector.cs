using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoinCollector : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new();

    public void SpendCoin(int amount)
    {
        TotalCoins.Value -= amount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Coin>(out Coin coin)) return;

        int coinValue = coin.Collect();

        if (!IsServer) return;

        TotalCoins.Value += coinValue;
    }

}
