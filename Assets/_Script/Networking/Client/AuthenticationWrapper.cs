using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthenticateState AuthState { get; private set; } = AuthenticateState.NotAuthenticated;

    public static async Task<AuthenticateState> DoAuth(int maxRetries = 5)
    {
        if (AuthState == AuthenticateState.Authenticated)
        {
            return AuthState;
        }

        if (AuthState == AuthenticateState.Authenticating)
        {
            Debug.LogWarning("Already Authenticating");
            await Authenticating();
            return AuthState;
        }
        await SignInAnonymouslyAsync(maxRetries);

        return AuthState;

    }

    static async Task<AuthenticateState> Authenticating()
    {
        while (AuthState == AuthenticateState.Authenticating || AuthState == AuthenticateState.NotAuthenticated)
        {
            await Task.Delay(200);
        }

        return AuthState;
    }

    static async Task SignInAnonymouslyAsync(int maxRetries)
    {
        AuthState = AuthenticateState.Authenticating;

        int reTries = 0;
        while (AuthState == AuthenticateState.Authenticating && reTries < maxRetries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthenticateState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException authEx)
            {
                Debug.LogError(authEx);
                AuthState = AuthenticateState.Error;
            }
            catch (RequestFailedException requestEx)
            {
                Debug.LogError(requestEx);
                AuthState = AuthenticateState.Error;
            }

            reTries++;
            // if not successed, wait 1000 ms then try again
            await Task.Delay(1000);
        }

        if (AuthState != AuthenticateState.Authenticated)
        {
            Debug.LogWarning("Login Times Out");
            AuthState = AuthenticateState.TimeOut;
        }
    }







}

public enum AuthenticateState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}
