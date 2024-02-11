using UnityEngine;
using Unity.Netcode;


public class PlayerMovement : NetworkBehaviour
{
    [Header("Reference")]
    [SerializeField] SO_InputReader _inputReader;
    [SerializeField] Transform _bodyTransform;
    [SerializeField] Rigidbody2D _rb;

    [Header("Settings")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateRate;

    Vector2 _movementInput;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _inputReader.PrimaryMoveEvent += MoveEvent;
    }


    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.PrimaryMoveEvent -= MoveEvent;
    }

    void Update()
    {
        if (!IsOwner) return;

        float zRotation = _movementInput.x * -_rotateRate * Time.deltaTime;
        _bodyTransform.Rotate(0f, 0f, zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        _rb.velocity = (Vector2)_bodyTransform.up * _movementInput.y * _moveSpeed;

    }

    private void MoveEvent(Vector2 movementInput)
    {
        _movementInput = movementInput;
    }
}
