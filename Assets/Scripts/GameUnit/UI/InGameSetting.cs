using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGameSetting : Setting
{
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button exitBtn;

    // Start is called before the first frame update
    void Start()
    {
        InitValue(Managers.Game.BgmValue, Managers.Game.EffValue, Managers.Game.BgmisOn, Managers.Game.EffisOn);

        if (retryBtn != null)
            retryBtn.onClick.AddListener(InGameRetry);

        if (exitBtn != null)
            exitBtn.onClick.AddListener(InGameExit);



        if (closeBtn != null)
            closeBtn.onClick.AddListener(CloseSettingPopUp);
    }

    // Update is called once per frame
    void Update()
    {
        ToggleSoundMute(Define.Sound.BGM, Define.Sound.Effect);
        SliderSound(Define.Sound.BGM, Define.Sound.Effect);
    }

    void InGameRetry()
    {
        Managers.Scene.LoadScene(Define.Scene.BattleStage_Field);
    }

    void InGameExit()
    {
        Managers.Scene.LoadScene(Define.Scene.Lobby);
    }
}
