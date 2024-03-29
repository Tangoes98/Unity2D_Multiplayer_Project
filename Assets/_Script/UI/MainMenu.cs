using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField _joinCodeField;
    public async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync();

    }

    public async void StartClient()
    {
        await ClientSingleton.Instance.GameManager.StartClientAsync(_joinCodeField.text);
    }
}
