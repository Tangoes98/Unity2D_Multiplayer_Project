using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    const string MENU_SCENE_NAME = "Menu";

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



}
