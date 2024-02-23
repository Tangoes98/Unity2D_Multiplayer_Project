using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] ClientSingleton _clientPrefab;
    [SerializeField] HostSingleton _hostPrefab;

    async void Start()
    {
        DontDestroyOnLoad(this);

        await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    async Task LaunchInMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {

        }
        else
        {
            HostSingleton hostSingleton = Instantiate(_hostPrefab);
            hostSingleton.CreateHost();

            ClientSingleton clientSingleton = Instantiate(_clientPrefab);
            bool isAuthenticated = await clientSingleton.CreateClient();

            if(isAuthenticated)
            {
                clientSingleton.GameManager.GoToMenu();
            }

            // Go to main menu
        }
    }
}
