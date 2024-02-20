using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    static ClientSingleton _insatnce;
    public static ClientSingleton Instance
    {
        get
        {
            if (_insatnce != null) return _insatnce;

            _insatnce = FindObjectOfType<ClientSingleton>();

            if (_insatnce == null)
            {
                Debug.LogError("No ClientSingleton in the scene");
                return null;
            }

            return _insatnce;
        }
    }

    public ClientGameManager GameManager { get; private set; }



    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public async Task<bool> CreateClient()
    {
        GameManager = new();
        return await GameManager.InitAsync();
    }
}
