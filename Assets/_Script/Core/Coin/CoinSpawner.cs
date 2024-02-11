using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] RespawnCoin _coinPrefab;

    [SerializeField] int _maxCoin;
    [SerializeField] int _coinValue;
    [SerializeField] Vector2 _xSpawnRange;
    [SerializeField] Vector2 _ySpawnRange;
    [SerializeField] LayerMask _coinLayer;

    float _coinRadius;
    CircleCollider2D[] _coinBuffer = new CircleCollider2D[1];

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        _coinRadius = _coinPrefab.GetComponent<CircleCollider2D>().radius;

        for (int i = 0; i < _maxCoin; i++)
        {
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        var coin = Instantiate(_coinPrefab, CoinSpawnPosition(), Quaternion.identity);

        coin.SetValue(_coinValue);
        coin.GetComponent<NetworkObject>().Spawn();

        coin.OnCoinCollected += OnCoinCollectedEvent;
    }

    Vector2 CoinSpawnPosition()
    {
        float x = 0f;
        float y = 0f;
        while (true)
        {
            x = Random.Range(_xSpawnRange.x, _xSpawnRange.y);
            y = Random.Range(_ySpawnRange.x, _ySpawnRange.y);
            Vector2 spawnPosition = new Vector2(x, y);

            int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPosition, _coinRadius, _coinBuffer, _coinLayer);
            if (numColliders == 0) return spawnPosition;
        }
    }

    void OnCoinCollectedEvent(RespawnCoin reCoin)
    {
        reCoin.transform.position = CoinSpawnPosition();
        reCoin.Reset();
    }


}
