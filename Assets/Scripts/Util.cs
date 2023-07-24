using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static T ComponentInit<T>(string name,out T compo) where T : Behaviour
    {
        GameObject obj = null;
        obj = GameObject.Find(name);

        if (obj == null)
        {
            compo = null;
            return compo;
        }

        if (obj.TryGetComponent(out compo))
            return compo;
        else
            return null;
    }




}
