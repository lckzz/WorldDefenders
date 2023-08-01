using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static bool s_init = false;


    UIManager _ui = new UIManager();
    GameManager _game = new GameManager();
    public static UIManager UI { get { return Instance?._ui; } }
    public static GameManager Game { get { return Instance?._game; } }


    public static Managers Instance
    {
        get
        {
            if (s_init == false)
            {
                s_init = true;

                GameObject go = GameObject.Find("Managers");
                if (go == null)
                {
                    go = new GameObject { name = "Managers" };
                    go.AddComponent<Managers>();
                }

                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<Managers>();
            }

            return s_instance;
        }

    }

    private void Start()
    {
        Debug.Log(_game);

    }

}
