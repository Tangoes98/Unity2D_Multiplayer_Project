using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCoin : Coin
{

    public event Action<RespawnCoin> OnCoinCollected;

    Vector3 _previousPosition;

    private void Update()
    {
        if (_previousPosition != transform.position)
        {
            Show(true);
        }
        _previousPosition = transform.position;
    }

    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }

        if (_alreadyCollected) return 0;

        _alreadyCollected = true;

        OnCoinCollected?.Invoke(this);

        return _coinValue;
    }

    public void Reset() => _alreadyCollected = false;
}
