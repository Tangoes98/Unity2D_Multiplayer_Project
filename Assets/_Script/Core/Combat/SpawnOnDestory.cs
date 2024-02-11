using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestory : MonoBehaviour
{
    [SerializeField] GameObject _prefab;

    private void OnDestroy()
    {
        Instantiate(_prefab, transform.position, Quaternion.identity);
    }
}
