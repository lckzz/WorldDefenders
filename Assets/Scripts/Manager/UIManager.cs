using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class UIManager
{
    public UI_Base _ui;
    Stack<UI_Base> _uiStack = new Stack<UI_Base>();


    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("UI_Root");
            if(root == null)
            {
                root = new GameObject { name = "UI_Root" };
            }

            return root;
        }
    }

    public T GetSceneUI<T>() where T : UI_Base
    {
        return _ui as T;
    }

    public T ShowSceneUI<T>() where T : UI_Base
    {
        if (_ui != null)
            return GetSceneUI<T>();

        string key = typeof(T).Name;
        T ui = null;
        GameObject.Find(key).TryGetComponent(out ui);
        _ui = ui;
        return ui;
    }

    public void OnOffSceneUI<T>(bool onoff) where T : UI_Base
    { 

        _ui.gameObject.SetActive(onoff);
    }


    public T ShowPopUp<T>(string name = null) where T : UI_Base
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject prefab = Resources.Load<GameObject>($"Prefabs/PopUp/{name}");
        GameObject go = Object.Instantiate(prefab);

        T popup = null;
        go.TryGetComponent<T>(out popup);
        _uiStack.Push(popup);           //씬넘어가면 스택초기화

        go.transform.SetParent(Root.transform);

        go.transform.localScale = Vector3.one;
        go.transform.localPosition = prefab.transform.position;

        return popup;
    }

  

    public void ClosePopUp(UI_Base popUp) 
    {
        if (_uiStack.Count == 0)
            return;
        if (_uiStack.Peek() != popUp)
            return;

        ClosePopUp();
    }

    public void ClosePopUp()
    {
        if (_uiStack.Count == 0)
            return;

        UI_Base popUp = _uiStack.Pop();
        Managers.Resource.Destory(popUp.gameObject);
        popUp = null;
        _ui = null;
    }

    public T FindPopup<T>() where T : UI_Base
    {
        return _uiStack.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
    }

    public T PeekPopupUI<T>() where T : UI_Base
    {
        if (_uiStack.Count == 0)
            return null;

        return _uiStack.Peek() as T;
    }


    public void CloseAllPopUpUI()
    {
        while (_uiStack.Count > 0 && _uiStack.Peek() != null)
            ClosePopUp();
    }

    public void Clear()
    {
        CloseAllPopUpUI();
    }
 
}
