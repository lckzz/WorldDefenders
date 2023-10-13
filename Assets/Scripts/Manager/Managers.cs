using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init();  return s_instance; } }

    PoolManager _pool = new PoolManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();
    ResourceManager _resource = new ResourceManager();
    DataManager _data = new DataManager();
    SoundManager _sound = new SoundManager();
    LoadingSceneManager _loading = new LoadingSceneManager();
    GameManagerEx _game = new GameManagerEx();


    public static PoolManager Pool { get { return Instance?._pool; } }
    public static SceneManagerEx Scene { get { return Instance?._scene; } }
    public static UIManager UI { get { return Instance?._ui; } }
    public static GameManagerEx Game { get { return Instance?._game; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static SoundManager Sound { get { return Instance?._sound; } }
    public static DataManager Data { get { return Instance?._data; } }
    public static LoadingSceneManager Loading { get { return Instance?._loading; } }

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

            s_instance._pool.Init();
            s_instance._sound.Init();
            s_instance._data.Init();

        }
    }

  


    public static void Clear()
    {
        Sound.Clear();
        UI.Clear();
        Scene.Clear();


        Pool.Clear();
    }




}
