using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager _instance = null;
    public static Manager Instance { get { return _instance; } }

    NetworkManager _networkManager = new NetworkManager();
    public NetworkManager NetworkManager { get { return _networkManager; } }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Manager>();
            }

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Manager>();
            _instance._networkManager.Init();
        }
        

    }
}
