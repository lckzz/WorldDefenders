using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    UI_Base _ui;
    Stack<UI_Base> _uiStack = new Stack<UI_Base>();

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
}
