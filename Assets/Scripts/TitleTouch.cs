using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleTouch : MonoBehaviour
{
    private Image touchPanelImg;

    private void Start()
    {
        TryGetComponent(out touchPanelImg);
    }

    public void TouchOnPanel()
    {
        touchPanelImg.DOFade(1, 1.0f).OnComplete(SceneMove);
    }


    void SceneMove()
    {
        Managers.Scene.Clear();
        Managers.Sound.Clear();
        Managers.Loading.LoadScene(Define.Scene.Lobby);
    }
}
