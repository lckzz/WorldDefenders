using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager 
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if(prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public GameObject MagicEffectAndSound(Vector3 pos, string soundPath, string effPath)
    {
        //Managers.Sound.Play($"Sounds/Effect/{soundPath}");
        GameObject eff = Managers.Resource.Load<GameObject>($"Prefabs/Effect/{effPath}");


        if (eff != null)
            GameObject.Instantiate(eff, pos, Quaternion.identity);

        return eff;
    }

    public void Destory(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
