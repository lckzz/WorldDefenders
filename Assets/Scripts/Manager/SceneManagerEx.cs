using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene _scene;

    public BaseScene CurrentScene{ get { return GameObject.FindObjectOfType<BaseScene>(); }}

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
        Time.timeScale = 1.0f;
    }

    public AsyncOperation LoadSceneAsync(Define.Scene type)
    {
        //Managers.Clear();
        return SceneManager.LoadSceneAsync(GetSceneName(type));
    }


    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    


    public void Clear()
    {
        CurrentScene.Clear();

    }
}
