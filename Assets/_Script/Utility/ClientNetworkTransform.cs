using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }
    protected override void Update()
    {
        CanCommitToTransform = IsOwner;
        base.Update();
        // if (NetworkManager != null)
        // {
        //     if (NetworkManager.IsConnectedClient || NetworkManager.IsListening)
        //     {
        //         if (CanCommitToTransform)
        //         {
        //             TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
        //         }
        //     }
        // }

        if (NetworkManager == null) return;
        if (!NetworkManager.IsConnectedClient || !NetworkManager.IsListening) return;
        if (!CanCommitToTransform) return;
        TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
    }
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
