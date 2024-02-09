using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] SO_InputReader _inputReader;
    [SerializeField] Transform _turrentTransform;

    private void LateUpdate()
    {
        if (!IsOwner) return;

        Vector2 aimScreenPosition = _inputReader.AimPosition;
        Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(aimScreenPosition);

        Vector2 directionVector = aimWorldPosition - (Vector2)_turrentTransform.position;
        _turrentTransform.up = directionVector;

    }





}
