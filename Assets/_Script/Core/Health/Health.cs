using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> CurrentHealth = new();

    [field: SerializeField] public int MaxHealth { get; private set; }

    bool _isDead;
    public Action<Health> OnDie;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damage) => ModifyHealth(-damage);

    public void RestoreHealth(int heal) => ModifyHealth(heal);

    void ModifyHealth(int value)
    {
        if (_isDead) return;

        int newHealth = CurrentHealth.Value + value;
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);

        if (CurrentHealth.Value == 0)
        {
            OnDie?.Invoke(this);
            _isDead = true;
        }

    }


}
