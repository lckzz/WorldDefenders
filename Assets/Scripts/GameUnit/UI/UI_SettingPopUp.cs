using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingPopUp : UI_Base
{
    [SerializeField] private GameObject lobbySettingCanvas;
    [SerializeField] private GameObject inGameSettingCanvas;



    public void SettingType(Define.SettingType type)
    {
        if (type == Define.SettingType.LobbySetting)
        {
            lobbySettingCanvas.SetActive(true);
            inGameSettingCanvas.SetActive(false);
        }
        else
        {
            lobbySettingCanvas.SetActive(false);
            inGameSettingCanvas.SetActive(true);
        }

    }


}
