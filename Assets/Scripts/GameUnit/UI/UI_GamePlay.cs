using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] Image fadeImg;

    [SerializeField] Button settingBtn;



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
    [SerializeField] Button skillBtn;
    [SerializeField] Image skillImg;
    [SerializeField] Image skillCoolImg;
    [SerializeField] Sprite[] skillsprites;
    [SerializeField] GameObject skillParticleObj;

    SkillData skillData;
    bool skillOnOff = false;
    float skillCoolTime = .0f;
    float skillCool = .0f;


    [SerializeField] private GameObject skillDurationObj;
    [SerializeField] private Image skillDurationImg;
    [SerializeField] private Image skillDurationTimeImg;
    [SerializeField] private TextMeshProUGUI skillDurationTimeTxt;
    private float durationTime;
    //----------Game SkillSet---------------------

    bool fadeCheck = true;

    //----------DOTween MovePos--------------
    float uiAttBtnBeforePosY = -580.0f;
    float uiAttBtnAfterPosY = -280.0f;

    float uiSkillBtnBeforePosY = -435.0f;
    float uiSkillBtnAfterPosY = -150.0f;

    float uiCostBeforePosY = -130.0f;
    float uiCostAfterPosY = 70.0f;
    //----------DOTween MoveSetting--------------



    bool leftBtnCheck = false;      //왼쪽 움직임 버튼을 눌럿는지 판단하는 변수
    bool rightBtnCheck = false;     //오른쪽 움직임 버튼을 눌럿는지 판단하는 변수

    public bool LeftBtnCheck { get { return leftBtnCheck; }}
    public bool RightBtnCheck { get { return rightBtnCheck; } }


    UnitNode unitNode;
    GameObject unitObj;
    GameObject unitSpawnBtnPrefab;

    [SerializeField] private Vector3[] unitNodePos;
    Vector3 unitNodeStartPos = new Vector3(105.0f, -260.0f, .0f);

    public SkillBook Skills { get; protected set; }


    public Queue<GameObject> gameQueue = new Queue<GameObject>();
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




        for (int ii = 0; ii < GlobalData.g_SlotUnitClass.Count; ii++)
        {
            if (GlobalData.g_SlotUnitClass[ii] != UnitClass.Count)
            {
                GameObject nodeObj = Managers.Resource.Instantiate("Unit/UnitSpawnBtn", contents.transform);
                nodeObj.TryGetComponent(out UnitNode unitNode);
                unitNode.UnitInit(GlobalData.g_SlotUnitClass[ii]);
                

            }    
        }

       
        uiUnit = new Button[contents.transform.childCount];

        for(int ii = 0; ii < uiUnit.Length; ii++)
        {
            contents.transform.GetChild(ii).TryGetComponent(out uiUnit[ii]);
        }

        GameObject.Find("Player").TryGetComponent<PlayerController>(out player);
        player.transform.parent.gameObject.TryGetComponent(out playerTower);

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

        Skills = player.GetComponent<SkillBook>();


        if (GlobalData.g_CurPlayerEquipSkill == Define.PlayerSkill.Count)   //스킬이 장착되어있지않다면 스킬을 꺼주고
            skillBtn.gameObject.SetActive(false);
        
        else
        {
            skillBtn.gameObject.SetActive(true);    //스킬 장착이 되었다면 스킬을 켜준다.
            SkillInit();   //스킬 초기화
        }


        for (int i = 0; i < uiUnit.Length; i++)
        {
            uiUnit[i].TryGetComponent<UnitNode>(out UnitNode node);


            switch (node.Unit)
            {
                case UnitClass.Warrior:
                    if(GlobalData.g_UnitWarriorLv < 5)
                        UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/Warrior/WarriorUnitLv1");
                    else if(GlobalData.g_UnitWarriorLv >= 5)
                        UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/Warrior/WarriorUnitLv2");
                    break;

                case UnitClass.Archer:
                    if(GlobalData.g_UnitArcherLv < 5)
                        UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/Archer/ArcherUnitLv1");
                    else
                        UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/Archer/ArcherUnitLv2");

                    break;

                case UnitClass.Spear:
                    if(GlobalData.g_UnitSpearLv < 5)
                        UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/Spear/SpearUnitLv1");
                    else
                        UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/Spear/SpearUnitLv2");

                    break;

                case UnitClass.Priest:
                    if (GlobalData.g_UnitPriestLv < 5)
                        UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/Priest/PriestUnitLv1");
                    else
                        UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/Priest/PriestUnitLv2");

                    break;
                case UnitClass.Magician:
                    UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/MagicianUnit");
                    break;
                case UnitClass.Cavalry:
                    UnitButtonSetting(i, "Prefabs/Unit/InGameUnit/CavalryUnit");
                    break;
            }
        }

        StartUISet = StartCoroutine(StartUnitNodeUIMove());


        return true;
    }

    Coroutine StartUISet = null;

    private void OnEnable()
    {
        //Managers.UI.SceneUIClear();
        Managers.UI.ShowSceneUI<UI_GamePlay>();
    }


    private void Update()
    {
        UpdateSkill();
        FadeOut();
        UpdateCost(Managers.Game.Cost);
    }

    float delaySeconds = 0.1f;
    IEnumerator StartUnitNodeUIMove()
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


    void SkillInit()
    {
        skillsprites = new Sprite[(int)Define.PlayerSkill.Count];
        for(int ii = 0; ii < skillsprites.Length; ii++)
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
        skillImg.sprite = skillsprites[(int)GlobalData.g_CurPlayerEquipSkill];

        skillData = new SkillData();

        switch(GlobalData.g_CurPlayerEquipSkill)
        {
            case Define.PlayerSkill.Heal:
                skillData = Managers.Data.healSkillDict[(int)GlobalData.g_SkillHealLv];
                Skills.AddSkill<TowerHealSkill>();
                break;

            case Define.PlayerSkill.FireArrow:
                skillData = Managers.Data.fireArrowSkillDict[(int)GlobalData.g_SkillFireArrowLv];
                Skills.AddSkill<FireArrowSkill>();

                break;

            case Define.PlayerSkill.Weakness:
                skillData = Managers.Data.weaknessSkillDict[(int)GlobalData.g_SkillWeaknessLv];
                Skills.AddSkill<TowerHealSkill>();

                break;


        }
    }


    void ButtonEvent(GameObject obj,Action action = null, UIEvent type = UIEvent.PointerDown)
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

    void ButtonEvent1(GameObject obj, string path,int idx, Action<string,int> action = null, UIEvent type = UIEvent.PointerDown)
    {
        UI_EventHandler evt;
        obj.TryGetComponent(out evt);


        switch (type)
        {
            case UIEvent.PointerDown:
                evt.OnPointerDownUnitHandler -= (unUsedPath,unUsedIdx) => action(path,idx);
                evt.OnPointerDownUnitHandler += (unUsedPath,unUsedIdx) => action(path,idx);
                
                break;

        }

    }


    void UnitButtonSetting(int i , string path)
    {
        ButtonEvent1(uiUnit[i].gameObject, path, i, UnitSummonBtnClick, UIEvent.PointerDown);
    }

    void InGameSetting()
    {
        Debug.Log("wqeqwe");
        this.gameObject.SetActive(false);
        Managers.UI.ShowPopUp<UI_SettingPopUp>();
        Managers.UI.PeekPopupUI<UI_SettingPopUp>().SettingType(Define.SettingType.InGameSetting);
        Time.timeScale = 0.0f;
    }


    void CameraMoveColorInit()
    {
        clickOnMoveColor = new Color32(255, 255, 255, clickOnMoveColorAlpha);
        clickOffMoveColor = new Color32(255, 255, 255, clickOffMoveColorAlpha);

        if (leftCameraMoveBtn != null)
            leftCameraMoveBtn.TryGetComponent(out leftCameraMoveImg);

        if (rightCameraMoveBtn != null)
            rightCameraMoveBtn.TryGetComponent(out rightCameraMoveImg);
    }



    void FadeOut()
    {
        if (fadeCheck)
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
                    fadeCheck = false;

                }

            }
        }
    }

    void LeftBtnOn()
    {
        if (Managers.UI.PeekPopupUI<UI_SettingPopUp>() != null)
            return;

        leftBtnCheck = true;
        leftCameraMoveImg.color = clickOnMoveColor;
    }
    void LeftBtnOff()
    {
        if (Managers.UI.PeekPopupUI<UI_SettingPopUp>() != null)
            return;

        leftBtnCheck = false;
        leftCameraMoveImg.color = clickOffMoveColor;

    }

    void RightBtnOn()
    {
        if (Managers.UI.PeekPopupUI<UI_SettingPopUp>() != null)
            return;

        rightBtnCheck = true;
        rightCameraMoveImg.color = clickOnMoveColor;

    }
    void RightBtnOff()
    {
        if (Managers.UI.PeekPopupUI<UI_SettingPopUp>() != null)
            return;

        rightBtnCheck = false;
        rightCameraMoveImg.color = clickOffMoveColor;

    }


    void UpdateSkillCoolTimeSet()
    {
        if (skillOnOff)
            return;
        //스킬을 누르면 장착된 스킬이 나가게 할것 (스킬북을 통해서)
        if (Skills.activeSkillList.Count > 0)
        {
            Debug.Log("플레이어 스킬");
            switch(GlobalData.g_CurPlayerEquipSkill)
            {
                case Define.PlayerSkill.Heal:
                    Skills.activeSkillList[0].UseSkill(playerTower);     //스킬 사용
                    break;

                case Define.PlayerSkill.FireArrow:

                    Skills.activeSkillList[0].UseSkill(player);     //스킬 사용
                    break;

                case Define.PlayerSkill.Weakness:

                    break;
            }
            
        }
        //스킬을 누르면 해당 스킬의 쿨타임을 받고
        skillCoolTime = skillData.skillCoolTime;
        skillOnOff = true; //스킬이 나가서 쿨타임 시작
        skillCoolImg.gameObject.SetActive(skillOnOff);

        if(GlobalData.g_CurPlayerEquipSkill == Define.PlayerSkill.FireArrow || GlobalData.g_CurPlayerEquipSkill == Define.PlayerSkill.Weakness)
        {
            skillDurationObj.SetActive(true);
            skillDurationImg.sprite = skillsprites[(int)GlobalData.g_CurPlayerEquipSkill];
            StartCoroutine(SkillDurationTime());
        }


    }

    IEnumerator SkillDurationTime()
    {
        float duration = 0.0f;
        while(true)
        {
            skillDurationTimeTxt.text = ((int)player.DurationTime).ToString();
            duration = durationTime / skillData.skillValue;
            Debug.Log(duration);
            durationTime += Time.deltaTime;

            if (skillDurationTimeImg?.fillAmount < duration)
                skillDurationTimeImg.fillAmount += Time.deltaTime * 1.0f;


            if (duration >= .99f)
            {
                skillDurationTimeImg.fillAmount = 1;
                durationTime = 0.0f;
                skillDurationObj.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }

    void UpdateSkill()
    {
        if(skillOnOff)
        {

            skillCool = skillCoolTime / skillData.skillCoolTime;

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
        //Debug.Log(coolTime);

        if (coolImg.fillAmount > coolTime)
            coolImg.fillAmount -= Time.deltaTime * speed;

        if (coolTime >= 0.99f)
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



    //유닛 버튼을 누를시 소환
    public void UnitSummonBtnClick(string namePath,int idx)
    {
        unitObj = Resources.Load<GameObject>(namePath);

        uiUnit[idx].TryGetComponent(out unitNode);

        if (unitNode?.CoolCheck == false) //쿨타임이 돌고 있지않을 때
        {
            Managers.Game.UnitSummonEnqueue(unitObj, unitNode.UnitCost, unitNode);
        }
    }


    public void UiMove(RectTransform rt, float beforePosY , float afterPosY)
    {
        if (rt == null)
            return;


        if (rt.anchoredPosition.y == beforePosY)
            rt.DOLocalMoveY(afterPosY, 0.5f).SetEase(Ease.OutBack);
    }
}
