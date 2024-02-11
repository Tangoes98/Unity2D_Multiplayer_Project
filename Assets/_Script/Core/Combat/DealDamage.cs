using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] int _damage;

    ulong _ownerClientID;

    public void SetOwner(ulong clientID)
    {
        this._ownerClientID = clientID;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == null) return;

        if (other.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if (_ownerClientID == netObj.OwnerClientId) return;
        }

        if (other.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(_damage);
        }
    }
}

