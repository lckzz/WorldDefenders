using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Setting : MonoBehaviour
{
    [SerializeField] private Toggle bgmMuteToggle;
    [SerializeField] private Toggle effMuteToggle;
    [SerializeField] private Slider bgmSoundSlider;
    [SerializeField] private Slider effSoundSlider;

    [SerializeField] protected Button closeBtn;

    [SerializeField] private UI_SettingPopUp ui_Setting;

    //DefalutValue
    private float defalutBgmValue = 0.5f;                  //지금 설정된값이 디폴트값
    private float defalutEffValue = 0.5f;
    private bool defalutBgmisOn = false;
    private bool defalutEffisOn = false;
    //DefalutValue


    public void InitValue(float bgmSliderValue, float effSliderValue,bool bgmMuteisOn, bool effMuteisOn)
    {
        bgmSoundSlider.value = bgmSliderValue;
        effSoundSlider.value = effSliderValue;
        bgmMuteToggle.isOn = bgmMuteisOn;
        effMuteToggle.isOn = effMuteisOn;
    }

    public void DefalutSoundSet()                       //디폴트셋팅
    {
        GlobalData.g_BgmValue = defalutBgmValue;
        GlobalData.g_EffValue = defalutEffValue;
        GlobalData.g_BgmisOn = defalutBgmisOn;
        GlobalData.g_EffisOn = defalutEffisOn;


        bgmSoundSlider.value = GlobalData.g_BgmValue;
        effSoundSlider.value = GlobalData.g_EffValue;
        bgmMuteToggle.isOn = GlobalData.g_BgmisOn;
        effMuteToggle.isOn = GlobalData.g_EffisOn;
    }


    public void ToggleSoundMute(Define.Sound bgmType,Define.Sound effType)
    {
        GlobalData.g_BgmisOn = bgmMuteToggle.isOn;
        GlobalData.g_EffisOn = effMuteToggle.isOn;

        Managers.Sound.SoundMute(bgmType, GlobalData.g_BgmisOn);
        Managers.Sound.SoundMute(effType, GlobalData.g_EffisOn);

    }

    //public void ToggleEffSoundMute(Define.Sound type)
    //{
    //    Managers.Sound.SoundMute(type, effMuteToggle.isOn);
    //}

    public void SliderSound(Define.Sound bgmType, Define.Sound effType)
    {
        GlobalData.g_BgmValue = bgmSoundSlider.value;
        GlobalData.g_EffValue = effSoundSlider.value;


        Managers.Sound.SoundValue(bgmType, GlobalData.g_BgmValue);
        Managers.Sound.SoundValue(effType, GlobalData.g_EffValue);

    }

    public void SliderEffSound(Define.Sound type)
    {
        Managers.Sound.SoundValue(type, effSoundSlider.value);
    }


    public void CloseSettingPopUp()
    {
        Managers.UI.ClosePopUp(ui_Setting);
        if (Managers.Scene.CurrentScene is LobbyScene lobby)
        {
            lobby.LobbyUIOnOff(true);
            //lobby.LobbyTouchUnitInit();
        }

        if (Managers.Scene.CurrentScene is GameScene game)
        {
            game.UiOnOff(true);
            //lobby.LobbyTouchUnitInit();
        }


    }

}
