using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // NetworkManager
    NetworkManager _networkManager = new NetworkManager();
    public static NetworkManager NetworkManager { get { return Instance. _networkManager; } }

    // SceneManager
    SceneManager _sceneManager = new SceneManager();
    public static SceneManager SceneManager { get {return Instance. _sceneManager; } }

    // Manager
    static Manager _instance = null;
    public static Manager Instance { get { return _instance; } }

    void Start()
    {
        Init();
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

    void Update()
    {
        _networkManager.Update();
    }

}
