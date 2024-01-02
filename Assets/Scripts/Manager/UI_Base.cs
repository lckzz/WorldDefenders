using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    protected bool _init = false;
    protected bool startFadeOut = false;

    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Init();
    }


    public void GameObjectSiblingLastSet(GameObject go)
    {
        go.transform.SetAsLastSibling();        //해당 오브젝트를 가장 마지막에 배치
    }


}
