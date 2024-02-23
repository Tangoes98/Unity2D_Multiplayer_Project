using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    const int MAX_CONNECTIONS = 20;
    const string GAME_SCENE_NAME = "GameScene";
    Allocation _allocation;
    string _joinCode;
    string _lobbyId;


    public async Task StartHostAsync()
    {
        try
        {
            _allocation = await Relay.Instance.CreateAllocationAsync(MAX_CONNECTIONS);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return;
        }

        try
        {
            _joinCode = await Relay.Instance.GetJoinCodeAsync(_allocation.AllocationId);
            Debug.Log(_joinCode);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        RelayServerData relayServerData = new(_allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        //Add Lobby
        try
        {
            CreateLobbyOptions lobbyOptions = new();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {"JoinCode",
                new DataObject
                (
                    visibility: DataObject.VisibilityOptions.Member,
                    value: _joinCode
                )

                }
            };
            var lobby = await Lobbies.Instance.CreateLobbyAsync("MyLobby", MAX_CONNECTIONS, lobbyOptions);
            _lobbyId = lobby.Id;

            HostSingleton.Instance.StartCoroutine(LobbyHeartbeat(15));
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            return;
        }




        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.SceneManager.LoadScene(GAME_SCENE_NAME, LoadSceneMode.Single);


    }

    IEnumerator LobbyHeartbeat(float waitTime)
    {
        WaitForSecondsRealtime delay = new(waitTime);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(_lobbyId);
            yield return delay;
        }
    }







}
