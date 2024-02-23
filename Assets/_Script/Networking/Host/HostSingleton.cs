using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    static HostSingleton _insatnce;
    public static HostSingleton Instance
    {
        get
        {
            if (_insatnce != null) return _insatnce;

            _insatnce = FindObjectOfType<HostSingleton>();

            if (_insatnce == null)
            {
                Debug.LogError("No HostSingleton in the scene");
                return null;
            }

            return _insatnce;
        }
    }

    public HostGameManager GameManager { get; private set; }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void CreateHost()
    {
        GameManager = new();
    }
}
