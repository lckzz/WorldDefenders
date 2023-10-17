using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ResourceManager 
{
    public T Load<T>(string path) where T : Object
    {
        //오브젝트 풀에서 찾아서 있으면 
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);


            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }



        return Resources.Load<T>(path);
    }



    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        return Object.Instantiate(original, parent);
    }

    public GameObject Instantiate(GameObject original, Transform parent = null)
    {
        if (original == null)
        {
            Debug.Log($"Failed to load prefab null");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;


        return Object.Instantiate(original, parent);

    }

    public GameObject Instantiate(GameObject original,Vector3 instantPos,Transform parent = null)
    {
        if (original == null)
        {
            Debug.Log($"Failed to load prefab null");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            GameObject go = Managers.Pool.Pop(original, parent).gameObject;
            go.transform.position = instantPos;
            return go;
        }

        return Object.Instantiate(original, instantPos,Quaternion.identity);

    }

    public GameObject Instantiate(GameObject original, Vector3 instantPos, Quaternion rot, Transform parent = null)
    {
        if (original == null)
        {
            Debug.Log($"Failed to load prefab null");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            GameObject go = Managers.Pool.Pop(original, parent).gameObject;
            go.transform.position = instantPos;
            return go;
        }

        return Object.Instantiate(original, instantPos, rot,parent);

    }

    public GameObject ResourceEffect(Vector3 pos, string effPath,Transform parent = null)
    {
        //Managers.Sound.Play($"Sounds/Effect/{soundPath}");
        GameObject eff = Managers.Resource.Load<GameObject>($"Prefabs/Effect/{effPath}");


        if (eff.GetComponent<Poolable>() != null)
        {
            GameObject go = Managers.Pool.Pop(eff, parent).gameObject;
            go.transform.position = pos;
            return go;
        }

        if (eff != null)
            GameObject.Instantiate(eff, pos, Quaternion.identity);

        return eff;
    }

    public void ResourceSound(string soundPath)
    {
        Managers.Sound.Play($"Sounds/Effect/{soundPath}");

    }

    public GameObject ResourceEffectAndSound(Vector3 pos, string soundPath, string effPath,Transform parent = null)
    {
        Managers.Sound.Play($"Sounds/Effect/{soundPath}");
        GameObject eff = Managers.Resource.Load<GameObject>($"Prefabs/Effect/{effPath}");

        if (eff.GetComponent<Poolable>() != null)
        {
            GameObject go =  Managers.Pool.Pop(eff, parent).gameObject;
            go.transform.position = pos;
            return go;
        }

        if (eff != null)
            GameObject.Instantiate(eff, pos, Quaternion.identity);

        return eff;
    }

    public void Destroy(GameObject go,float time = .0f)
    {
        if (go == null)
            return;


        go.TryGetComponent(out Poolable poolable);

        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        if (time > .0f)
            Object.Destroy(go);
        else
            Object.Destroy(go, time);
    }


    
}
