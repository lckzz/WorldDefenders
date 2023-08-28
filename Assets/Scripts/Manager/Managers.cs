using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init();  return s_instance; } }

    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();
    GameManager _game = new GameManager();
    ResourceManager _resource = new ResourceManager();
    DataManager _data = new DataManager();
    SoundManager _sound = new SoundManager();
    SkillManager _skill = new SkillManager();

    public static SceneManagerEx Scene { get { return Instance?._scene; } }
    public static UIManager UI { get { return Instance?._ui; } }
    public static GameManager Game { get { return Instance?._game; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static SoundManager Sound { get { return Instance?._sound; } }
    public static DataManager Data { get { return Instance?._data; } }
    public static SkillManager Skill { get { return Instance?._skill; } }

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

            s_instance._sound.Init();
            s_instance._data.Init();

        }
    }

  

    private void Start()
    {
        Debug.Log(_game);

    }

    public static void Clear()
    {
        Sound.Clear();
        UI.Clear();
        Scene.Clear();
    }




}
