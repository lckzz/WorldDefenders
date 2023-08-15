using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public static bool FadeOut(bool fade, Image fadeImg)
    {
        if (fade)
        {
            if (!fadeImg.gameObject.activeSelf)
                fadeImg.gameObject.SetActive(true);

            if (fadeImg.gameObject.activeSelf && fadeImg.color.a > 0)
            {
                Color col = fadeImg.color;
                if (col.a > 0)
                    col.a -= (Time.deltaTime * 1.0f);

                fadeImg.color = col;

                if (fadeImg.color.a <= 0.01f)
                {
                    fadeImg.gameObject.SetActive(false);
                    fade = false;

                 
                }

            }

        }

        return fade;

    }

    public static void FadeIn(Image fadeImg, Define.Scene type)
    {
        if (fadeImg != null)
        {
            if (!fadeImg.gameObject.activeSelf)
            {
                fadeImg.gameObject.SetActive(true);

            }

            if (fadeImg.gameObject.activeSelf && fadeImg.color.a <= 1)
            {
                Color col = fadeImg.color;
                if (col.a < 255)
                    col.a += (Time.deltaTime * 1.0f);

                fadeImg.color = col;


                if (fadeImg.color.a >= 0.99f)
                {
                    Managers.Scene.LoadScene(type);


                }
            }
        }
    }


}
