using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;
using Random = UnityEngine.Random;

public enum UIEvent
{
    PointerDown,
    PointerUp
}

public class UI_GamePlay : UI_Base
{
    //인게임의 UI들의 기능들
    [SerializeField]
    Button uiAttackBtn;

    [SerializeField]
    GameObject contents; 
    [SerializeField]
    Button[] uiUnit;            //설정한 유닛들의 버튼정보

    [SerializeField]
    Image unitCost;
    [SerializeField]
    TextMeshProUGUI unitCostTxt;
    PlayerController player;
    Tower playerTower;
    [SerializeField]
    Image coolImg;
    [SerializeField] TextMeshProUGUI gameGoldTxt;
    [SerializeField] Image fadeImg;

    [SerializeField] Button settingBtn;

    [SerializeField] TextMeshProUGUI timerTxt;

    [SerializeField] GameObject warningTxtObj;





    //---------Camera Move Btn-----------------------------
    [SerializeField] Button leftCameraMoveBtn;
    [SerializeField] Button rightCameraMoveBtn;
    Image leftCameraMoveImg;
    Image rightCameraMoveImg;
    byte clickOnMoveColorAlpha = 255;
    byte clickOffMoveColorAlpha = 100;
    Color clickOnMoveColor;
    Color clickOffMoveColor;

    //---------Camera Move Btn-----------------------------


    //----------Game SkillSet---------------------
    [Header("-------------Skill---------------")]
    [SerializeField] private Button skillBtn;
    [SerializeField] private Image skillImg;
    [SerializeField] private Image skillCoolImg;
    [SerializeField] private Sprite[] skillsprites;
    [SerializeField] private GameObject skillParticleObj;
    private HealParticle towerHealParicle;

    private bool skillOnOff = false;
    private float skillCoolTime = .0f;
    private float skillCool = .0f;


    [SerializeField] private GameObject skillDurationObj;
    [SerializeField] private Image skillDurationImg;
    [SerializeField] private Image skillDurationTimeImg;
    [SerializeField] private TextMeshProUGUI skillDurationTimeTxt;
    [SerializeField] private TextMeshProUGUI fireSkillCountTxt;

    private Coroutine skillDurationCo = null;

    private float durationTime;
    //----------Game SkillSet---------------------



    //----------DOTween MovePos--------------
    private float uiAttBtnBeforePosY = -220.0f;
    private float uiAttBtnAfterPosY = 85.6f;

    private float uiSkillBtnBeforePosY = -75.0f;
    private float uiSkillBtnAfterPosY = 218.2f;

    private float uiCostBeforePosY = -130.0f;
    private float uiCostAfterPosY = 115.75f;
    //----------DOTween MoveSetting--------------



    private bool leftBtnCheck = false;      //왼쪽 움직임 버튼을 눌럿는지 판단하는 변수
    private bool rightBtnCheck = false;     //오른쪽 움직임 버튼을 눌럿는지 판단하는 변수

    public bool LeftBtnCheck { get { return leftBtnCheck; }}
    public bool RightBtnCheck { get { return rightBtnCheck; } }


    //private UnitNode unitNode;
    //private GameObject unitObj;


    [SerializeField] private Vector3[] unitNodePos;
    private Vector3 unitNodeStartPos = new Vector3(105.0f, -260.0f, .0f);



    private Coroutine StartUISet = null;

    //UI_EventHandler evt;

    // Start is called before the first frame update

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Game.MoneyCostInit();

        unitNodePos = new Vector3[5];


        for(int i = 0; i < unitNodePos.Length; i++)
        {
            unitNodePos[i] = unitNodeStartPos;
            unitNodeStartPos.x += 175.0f;
        }



        for (int ii = 0; ii < Managers.Game.SlotUnitClass.Count; ii++)
        {
            if (Managers.Game.SlotUnitClass[ii] != UnitClass.Count)
            {
                GameObject nodeObj = Managers.Resource.Instantiate("Unit/UnitSpawnBtn", contents.transform);
                nodeObj.TryGetComponent(out UnitNode unitNode);
                unitNode.UnitInit(Managers.Game.SlotUnitClass[ii]);
                

            }    
        }

       
        uiUnit = new Button[contents.transform.childCount];

        for(int ii = 0; ii < uiUnit.Length; ii++)
        {
            contents.transform.GetChild(ii).TryGetComponent(out uiUnit[ii]);
        }

        GameObject.Find("Player").TryGetComponent<PlayerController>(out player);
        player.transform.parent.gameObject.TryGetComponent(out playerTower);
        skillParticleObj.TryGetComponent(out towerHealParicle);

        if (settingBtn != null)
            settingBtn.onClick.AddListener(InGameSetting);

        ButtonEvent(uiAttackBtn.gameObject, player.AttackWait, UIEvent.PointerDown);
        ButtonEvent(uiAttackBtn.gameObject, player.ShotArrow, UIEvent.PointerUp);

        ButtonEvent(leftCameraMoveBtn.gameObject, LeftBtnOn, UIEvent.PointerDown);
        ButtonEvent(leftCameraMoveBtn.gameObject, LeftBtnOff, UIEvent.PointerUp);

        ButtonEvent(rightCameraMoveBtn.gameObject, RightBtnOn, UIEvent.PointerDown);
        ButtonEvent(rightCameraMoveBtn.gameObject, RightBtnOff, UIEvent.PointerUp);

        ButtonEvent(skillBtn.gameObject, UpdateSkillCoolTimeSet, UIEvent.PointerDown);

        CameraMoveColorInit();



        if (Managers.Game.CurPlayerEquipSkill == Define.PlayerSkill.Count)   //스킬이 장착되어있지않다면 스킬을 꺼주고
            skillBtn.gameObject.SetActive(false);
        
        else
        {
            skillBtn.gameObject.SetActive(true);    //스킬 장착이 되었다면 스킬을 켜준다.
            SkillUIInit();   //스킬 초기화
        }


        StartUISet = StartCoroutine(StartUnitNodeUIMove());


        return true;
    }


    private void OnEnable()
    {
        //Managers.UI.SceneUIClear();
        Managers.UI.ShowSceneUI<UI_GamePlay>();
    }

    private void OnDisable()
    {
        warningTxtObj?.SetActive(false);
    }


    private void Update()
    {
        UpdateSkill();
        //FadeOut();
        UpdateCost(Managers.Game.Cost);
    }

    private float delaySeconds = 0.1f;
    private IEnumerator StartUnitNodeUIMove()
    {
        WaitForSeconds wfs = new WaitForSeconds(delaySeconds);

        yield return wfs;


        RectTransform rt;
        uiAttackBtn.TryGetComponent(out rt);
        if (rt != null)
        {
            UiMove(rt, uiAttBtnBeforePosY, uiAttBtnAfterPosY);
        }

        skillBtn.TryGetComponent(out rt);
        if(rt != null)
        {
            UiMove(rt, uiSkillBtnBeforePosY, uiSkillBtnAfterPosY);
            yield return wfs;
        }

        for (int ii = 0; ii < uiUnit.Length; ii++)
        {
            uiUnit[ii].TryGetComponent(out UnitNode node);
            node.UnitPositionSet(unitNodePos[ii]);

            node.NodeMove();
            yield return wfs;

        }

        unitCost.TryGetComponent(out rt);
        if(rt != null)
            UiMove(rt, uiCostBeforePosY, uiCostAfterPosY);
       
            yield return wfs;
        

        yield break;
    }


    private void SkillUIInit()              //스킬 UI를 초기화시켜준다.
    {
        skillsprites = new Sprite[(int)Define.PlayerSkill.Count];
        for(int ii = 0; ii < skillsprites.Length; ii++)     //스킬이미지들을 넣어준다.
        {
            switch (ii)
            {
                case (int)Define.PlayerSkill.Heal:
                    skillsprites[ii] = Managers.Resource.Load<Sprite>("Sprite/Skill/Heal");
                    break;
                case (int)Define.PlayerSkill.FireArrow:
                    skillsprites[ii] = Managers.Resource.Load<Sprite>("Sprite/Skill/FireArrow");
                    break;
                case (int)Define.PlayerSkill.Weakness:
                    skillsprites[ii] = Managers.Resource.Load<Sprite>("Sprite/Skill/Weakness");
                    break;
            }
        }
        skillImg.sprite = skillsprites[(int)Managers.Game.CurPlayerEquipSkill];  //현재 장착하고있는 스킬들을 이미지에 넣어준다.

        
    }


    private void ButtonEvent(GameObject obj,Action action = null, UIEvent type = UIEvent.PointerDown)
    {
        UI_EventHandler evt;
        obj.TryGetComponent(out evt);

        switch(type)
        {
            case UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;

             case UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
        }
        
    }


    private void InGameSetting()
    {
        gameObject.SetActive(false);
        Managers.UI.ShowPopUp<UI_SettingPopUp>();
        Managers.UI.PeekPopupUI<UI_SettingPopUp>().SettingType(Define.SettingType.InGameSetting);
        Time.timeScale = 0.0f;
    }


    private void CameraMoveColorInit()
    {
        clickOnMoveColor = new Color32(255, 255, 255, clickOnMoveColorAlpha);
        clickOffMoveColor = new Color32(255, 255, 255, clickOffMoveColorAlpha);

        if (leftCameraMoveBtn != null)
            leftCameraMoveBtn.TryGetComponent(out leftCameraMoveImg);

        if (rightCameraMoveBtn != null)
            rightCameraMoveBtn.TryGetComponent(out rightCameraMoveImg);
    }



    private void LeftBtnOn()
    {
        if (Managers.UI.PeekPopupUI<UI_SettingPopUp>() != null)
            return;

        leftBtnCheck = true;
        leftCameraMoveImg.color = clickOnMoveColor;
    }
    private void LeftBtnOff()
    {
        if (Managers.UI.PeekPopupUI<UI_SettingPopUp>() != null)
            return;

        leftBtnCheck = false;
        leftCameraMoveImg.color = clickOffMoveColor;

    }

    private void RightBtnOn()
    {
        if (Managers.UI.PeekPopupUI<UI_SettingPopUp>() != null)
            return;

        rightBtnCheck = true;
        rightCameraMoveImg.color = clickOnMoveColor;

    }
    private void RightBtnOff()
    {
        if (Managers.UI.PeekPopupUI<UI_SettingPopUp>() != null)
            return;

        rightBtnCheck = false;
        rightCameraMoveImg.color = clickOffMoveColor;

    }


    private void UpdateSkillCoolTimeSet()  //스킬버튼을 누르면 행동
    {
        if (skillOnOff)
            return;

        player.ActiveSkillUse();                //플레이어에서 작동할 스킬을 사용한다.

        //스킬을 누르면 해당 스킬의 쿨타임을 받고
        skillCoolTime = player.SkillData.skillCoolTime;
        skillOnOff = true; //스킬이 나가서 쿨타임 시작
        skillCoolImg.gameObject.SetActive(skillOnOff);      //쿨타임을 켜준다.

        if(Managers.Game.CurPlayerEquipSkill != Define.PlayerSkill.Heal) //폭발화살일때 약화일때 지속시간을 보여준다.
        {
            skillDurationObj.SetActive(true);
            skillDurationImg.sprite = skillsprites[(int)Managers.Game.CurPlayerEquipSkill];             //지속시간을 켜주고 이미지도 현재 스킬로 바꿔준다.

            if (skillDurationCo != null)
                StopCoroutine(skillDurationCo);                         //지속시간 코루틴

            skillDurationCo = StartCoroutine(SkillDurationTime());

        }
        else
        {
            //성벽수리 사용시
            skillParticleObj.SetActive(true); 
        }


    }


    private WaitForSeconds wfs = new WaitForSeconds(0.1f);

    private IEnumerator SkillDurationTime()  //스킬 지속시간 UI 표시
    {
        float duration = 0.0f;

        yield return wfs;       //0.1초 대기해준다.


        while(true)
        {
            

            skillDurationTimeTxt.text = ((int)player.DurationTime).ToString();
            duration = durationTime / player.SkillData.skillDuration;
            durationTime += Time.deltaTime;

            if (skillDurationTimeImg?.fillAmount < duration)
                skillDurationTimeImg.fillAmount += Time.deltaTime * 1.0f;


            if(Managers.Game.CurPlayerEquipSkill == Define.PlayerSkill.FireArrow)
            {
                if (fireSkillCountTxt.gameObject.activeSelf == false)   //화살갯수를 켜준다.
                    fireSkillCountTxt.gameObject.SetActive(true);

                fireSkillCountTxt.text = player.FireArrowCount.ToString();


                if (duration >= .99f || player.FireArrowCount <= 0)
                {
                    skillDurationObj.SetActive(false);
                    skillDurationTimeImg.fillAmount = 0;
                    durationTime = 0.0f;
                    yield break;
                }
            }
            else if(Managers.Game.CurPlayerEquipSkill == Define.PlayerSkill.Weakness)
            {
                if (fireSkillCountTxt.gameObject.activeSelf == true)  //화살갯수를 꺼준다.
                    fireSkillCountTxt.gameObject.SetActive(false);

                if (duration >= .99f)
                {
                    skillDurationObj.SetActive(false);
                    skillDurationTimeImg.fillAmount = 0;
                    durationTime = 0.0f;
                    skillDurationTimeTxt.text = player.SkillData.skillDuration.ToString();
                    yield break;
                }
            }


            yield return null;
        }
    }

    private void UpdateSkill()
    {
        if(skillOnOff)
        {

            skillCool = skillCoolTime / player.SkillData.skillCoolTime;

            skillCoolTime -= Time.deltaTime;

            if (skillCoolImg?.fillAmount > skillCool)
                skillCoolImg.fillAmount -= Time.deltaTime * 1.0f;

            if (skillCool >= .99f)
                skillCoolImg.fillAmount = 1;

            if(skillCoolTime <= .0f)
            {
                skillCoolTime = .0f;
                skillOnOff = false;
                skillCoolImg.gameObject.SetActive(skillOnOff);
            }
        }
    }


    public void UpdateCoolTime(float speed)
    {
        float coolTime = player.PercentCoolTime();
        if (coolImg.fillAmount > coolTime)
            coolImg.fillAmount -= Time.deltaTime * speed;

        if (coolTime >= 0.9f)
            coolImg.fillAmount = 1;
    }

    public void UpdateUnitCoolTime(Image coolImg,float speed, float coolTime,float maxCoolTime)
    {
        float cool = coolTime / maxCoolTime;


        if (coolImg?.fillAmount > cool)
            coolImg.fillAmount -= Time.deltaTime * speed;

        if (cool >= .99f)
            coolImg.fillAmount = 1;


    }

    public void UpdateUnitCostEnable(Image img,Color grayColor, float unitCost,float gameCost)
    {
        if (gameCost < unitCost)
        {
            img.color = grayColor;      //매개변수에서 받은 컬러값
        }
        else
            img.color = new Color(1, 1, 1);
        

    }

    public void UpdateCost(float curCost)
    {
        unitCostTxt.text = curCost.ToString();

    }

    public void UpdateGold(float curGold)
    {
        gameGoldTxt.text = curGold.ToString();
    }

    public void InGameTimer()
    {
        int time = (int)Managers.Game.GetInGameTimer();
        int min = time / 60;
        int sec = time % 60;
        if (min >= 10 && sec < 10)
            timerTxt.text = $"{min} : 0{sec}";
        else if(min >= 10 && sec >= 10)
            timerTxt.text = $"{min} : {sec}";
        else if(min <10 && sec >= 10)
            timerTxt.text = $"0{min} : {sec}";
        else if(min < 10 && sec < 10)
            timerTxt.text = $"0{min} : 0{sec}";


    }



 


    public void UiMove(RectTransform rt, float beforePosY , float afterPosY)
    {
        if (rt == null)
            return;


        if (rt.anchoredPosition.y <= beforePosY)
            rt.DOAnchorPosY(afterPosY, 0.5f).SetEase(Ease.OutBack);
    }




}
