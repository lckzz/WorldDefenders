using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{

    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Init()
    {

    }

    public abstract void Clear();
}
