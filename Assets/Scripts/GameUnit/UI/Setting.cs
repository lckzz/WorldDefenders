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

        Managers.Game.BgmValue = defalutBgmValue;
        Managers.Game.EffValue = defalutEffValue;
        Managers.Game.BgmisOn = defalutBgmisOn;
        Managers.Game.EffisOn = defalutEffisOn;


        bgmSoundSlider.value = Managers.Game.BgmValue;
        effSoundSlider.value = Managers.Game.EffValue;
        bgmMuteToggle.isOn = Managers.Game.BgmisOn;
        effMuteToggle.isOn = Managers.Game.EffisOn;
    }


    public void ToggleSoundMute(Define.Sound bgmType,Define.Sound effType)
    {
        //if((Managers.Game.BgmisOn != bgmMuteToggle.isOn) || (Managers.Game.EffisOn != effMuteToggle.isOn))
        //    Managers.Sound.Play("Effect/UI_Click");


        Managers.Game.BgmisOn = bgmMuteToggle.isOn;
        Managers.Game.EffisOn = effMuteToggle.isOn;

        Managers.Sound.SoundMute(bgmType, Managers.Game.BgmisOn);
        Managers.Sound.SoundMute(effType, Managers.Game.EffisOn);

    }

    //public void ToggleEffSoundMute(Define.Sound type)
    //{
    //    Managers.Sound.SoundMute(type, effMuteToggle.isOn);
    //}

    public void SliderSound(Define.Sound bgmType, Define.Sound effType)
    {
        Managers.Game.BgmValue = bgmSoundSlider.value;
        Managers.Game.EffValue = effSoundSlider.value;


        Managers.Sound.SoundValue(bgmType, Managers.Game.BgmValue);
        Managers.Sound.SoundValue(effType, Managers.Game.EffValue);

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Managers.Sound.Play("Effect/UI_Click");

        ToggleSoundMute(Define.Sound.BGM, Define.Sound.Effect);
        SliderSound(Define.Sound.BGM, Define.Sound.Effect);
    }

}
