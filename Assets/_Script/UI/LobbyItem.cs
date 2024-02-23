using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] TMP_Text _lobbyNameText;
    [SerializeField] TMP_Text _lobbyPlayerText;

    LobbyList _lobbyList;
    Lobby _lobby;
    public void Initialize(LobbyList lobbyList, Lobby lobby)
    {
        _lobbyList = lobbyList;
        _lobby = lobby;

        _lobbyNameText.text = lobby.Name;
        _lobbyPlayerText.text = $"{lobby.Players.Count}/ {lobby.MaxPlayers}";
    }

    public void Join()
    {
        _lobbyList.JoinAsync(_lobby);
    }

}
