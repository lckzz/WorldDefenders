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
        //����� ������ ���� ���ҰŰ��� �޾Ƽ� �κ������ ���������� ��������
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
