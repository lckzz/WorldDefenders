using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySetting : Setting
{
    [SerializeField] private Button dataInitBtn;
    [SerializeField] private Button settingDefalutBtn;

    // Start is called before the first frame update
    void Start()
    {
        //저장된 음량의 값과 음소거값을 받아서 로비셋팅이 켜질때마다 셋팅해줌
        InitValue(GlobalData.g_BgmValue, GlobalData.g_EffValue, GlobalData.g_BgmisOn, GlobalData.g_EffisOn);

        if (settingDefalutBtn != null)
            settingDefalutBtn.onClick.AddListener(DefalutSoundSet);

        if (closeBtn != null)
            closeBtn.onClick.AddListener(CloseSettingPopUp);
    }

    // Update is called once per frame
    void Update()
    {
        ToggleSoundMute(Define.Sound.BGM, Define.Sound.Effect);
        SliderSound(Define.Sound.BGM, Define.Sound.Effect);

    }




    
}
