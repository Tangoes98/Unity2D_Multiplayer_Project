using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    const string MENU_SCENE_NAME = "Menu";

    JoinAllocation _joinAllocation;

    public async Task<bool> InitAsync()
    {
        // Authenticate player
        // Initialize unity gaming services

        await UnityServices.InitializeAsync();

        AuthenticateState authState = await AuthenticationWrapper.DoAuth();

        if (authState == AuthenticateState.Authenticated)
        {
            return true;
        }
        else return false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }

    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            _joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        RelayServerData relayServerData = new(_joinAllocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
    }
}
