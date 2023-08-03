using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init();  return s_instance; } }


    UIManager _ui = new UIManager();
    GameManager _game = new GameManager();
    ResourceManager _resource = new ResourceManager();
    DataManager _data = new DataManager();

    public static UIManager UI { get { return Instance?._ui; } }
    public static GameManager Game { get { return Instance?._game; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static DataManager Data { get { return Instance?._data; } }


    static void Init()
    {
        if(s_instance == null)
        {

            GameObject go = GameObject.Find("Managers");
            if (go == null)
            {
                go = new GameObject { name = "Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._data.Init();

        }
    }

  

    private void Start()
    {
        Debug.Log(_game);

    }

}
