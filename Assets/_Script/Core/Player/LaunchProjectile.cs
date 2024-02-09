using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class LaunchProjectile : NetworkBehaviour
{
    [Header("Reference")]
    [SerializeField] SO_InputReader _InputReader;
    [SerializeField] Transform _spawnPosition;
    [SerializeField] GameObject _serverProjectile;
    [SerializeField] GameObject _clientProjectile;
    [SerializeField] GameObject _muzzleFlash;
    [SerializeField] Collider2D _playerCollider;

    [Header("Settings")]
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _fireCD;
    [SerializeField] float _muzzleFlashDuration;
    bool _shouldFire;
    float _muzzleFlashTimer;
    float _fireTimer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _InputReader.PrimaryFireEvent += FireProjectileEvent;
    }


    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _InputReader.PrimaryFireEvent -= FireProjectileEvent;
    }



    private void Update()
    {
        MuzzleFlashTimerCheck();
        if (!CanPlayerLaunchProjectile()) return;

        if (!IsOwner) return;
        if (!_shouldFire) return;

        SpawnsServerProjectile_ServerRpc(_spawnPosition.transform.position, _spawnPosition.up);
        SpawnProjectile(_clientProjectile, _spawnPosition.transform.position, _spawnPosition.up);


    }

    void FireProjectileEvent(bool shouldFire)
    {
        _shouldFire = shouldFire;
    }

    [ServerRpc]
    void SpawnsServerProjectile_ServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        SpawnProjectile(_serverProjectile, spawnPos, direction);
        SpawnClientProjectile_ClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    void SpawnClientProjectile_ClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner) return;
        SpawnProjectile(_clientProjectile, spawnPos, direction);
    }

    void SpawnProjectile(GameObject projectile, Vector3 spawnPos, Vector3 direction)
    {
        _muzzleFlash.SetActive(true);
        _muzzleFlashTimer = _muzzleFlashDuration;
        _fireTimer = _fireCD;

        GameObject projectileInstance = Instantiate(projectile, spawnPos, quaternion.identity);

        projectileInstance.transform.up = direction;

        Physics2D.IgnoreCollision(_playerCollider, projectileInstance.GetComponent<Collider2D>());

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = direction * _projectileSpeed;
        }
    }

    void MuzzleFlashTimerCheck()
    {
        if (_muzzleFlashTimer > 0)
        {
            _muzzleFlashTimer -= Time.deltaTime;
        }
        else
        {
            _muzzleFlashTimer = 0f;
            _muzzleFlash.SetActive(false);
        }
    }

    bool CanPlayerLaunchProjectile()
    {
        if (_fireTimer > 0)
        {
            _fireTimer -= Time.deltaTime;
            return false;
        }
        else return true;
    }
}
