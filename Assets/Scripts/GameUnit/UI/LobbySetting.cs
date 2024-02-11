using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbySetting : Setting
{
    [SerializeField] private Button dataInitBtn;
    [SerializeField] private Button settingDefalutBtn;
    [SerializeField] private TMP_Dropdown resoultionDropDown;
    [SerializeField] private Toggle dropDownToggle;

    private FullScreenMode fullScreenMode;
    [SerializeField] private int resolutionNum = 0;
    private Resolution resolution;

    private List<Resolution> resolutions = new List<Resolution>();
    // Start is called before the first frame update
    void Start()
    {
        //저장된 음량의 값과 음소거값을 받아서 로비셋팅이 켜질때마다 셋팅해줌
        InitValue(Managers.Game.BgmValue, Managers.Game.EffValue, Managers.Game.BgmisOn, Managers.Game.EffisOn);

        if (settingDefalutBtn != null)
            settingDefalutBtn.onClick.AddListener(DefalutSoundSet);

        if (closeBtn != null)
            closeBtn.onClick.AddListener(DropDownOkBtnClick);

        ResolutionInit();

        if (resoultionDropDown != null)
            resoultionDropDown.onValueChanged.AddListener(DropDownOptionChange);

        if (dropDownToggle != null)
            dropDownToggle.onValueChanged.AddListener(FullScreenBtn);
    }

    private int[,] screenResolutions = new int[,]
    {
        {1280,720 },
        {1366,768 },
        {1920,1080 },
        {2560,1440 }

    };

    void ResolutionInit()
    {

        //for (int ii = 0; ii < Screen.resolutions.Length; ii++)
        //{
        //    if (Screen.resolutions[ii].refreshRateRatio.ToString() == "60")
        //    {
        //        if (Screen.resolutions[ii].width == 1280 && Screen.resolutions[ii].height == 720)
        //            resolutions.Add(Screen.resolutions[ii]);
        //        if (Screen.resolutions[ii].width == 1920 && Screen.resolutions[ii].height == 1080)
        //            resolutions.Add(Screen.resolutions[ii]);
        //        if (Screen.resolutions[ii].width == 2560 && Screen.resolutions[ii].height == 1440)
        //            resolutions.Add(Screen.resolutions[ii]);
        //        if (Screen.resolutions[ii].width == 1366 && Screen.resolutions[ii].height == 768)
        //            resolutions.Add(Screen.resolutions[ii]);


        //    }
        //}

        for(int ii = 0; ii < screenResolutions.GetLength(0);ii++)
             ResolutionSetAdd(screenResolutions[ii,0],screenResolutions[ii,1]);
        





        resoultionDropDown.options.Clear();
        
        resolutionNum = 0;
        //Debug.Log(resolutions.Count);

        //string[] refreshRateTxts;
        //string refreshRateTxt;

        foreach(Resolution re in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            //refreshRateTxts = re.refreshRateRatio.ToString().Split('.');
            //if (refreshRateTxts.Length > 1)
            //    refreshRateTxt = refreshRateTxts[0];
            //else
            //    refreshRateTxt = re.refreshRateRatio.ToString();
            option.text = re.width + "x" + re.height;
            resoultionDropDown.options.Add(option);

            if (re.width == Screen.width && re.height == Screen.height)
                resoultionDropDown.value = resolutionNum;

            else
                resolutionNum++;
        }

        resoultionDropDown.RefreshShownValue();

        dropDownToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    void ResolutionSetAdd(int width, int height)
    {
        resolution.width = width;
        resolution.height = height;
        resolutions.Add(resolution);
    }

    public void DropDownOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void DropDownOkBtnClick()
    {
        fullScreenMode = dropDownToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        resolutionNum = resoultionDropDown.value;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, fullScreenMode);
        CloseSettingPopUp();

    }

    // Update is called once per frame
    //void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //        Managers.Sound.Play("Effect/UI_Click");

    //    ToggleSoundMute(Define.Sound.BGM, Define.Sound.Effect);
    //    SliderSound(Define.Sound.BGM, Define.Sound.Effect);

    //}





}
