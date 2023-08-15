using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum UIEvnet
{
    PointerDown,
    PointerUp
}

public class UI_GamePlay : UI_Base
{
    //인게임의 UI들의 기능들
    [SerializeField]
    Button uiUnitSword;         //나중에 유저가 선택한 유닛들의 갯수를 받아서 버튼배열로 처리해야함
    [SerializeField]
    Button uiUnitBow;
    [SerializeField]
    Button uiAttackBtn;

    [SerializeField]
    GameObject contents;

    Button[] uiUnit;            //설정한 유닛들의 버튼정보

    [SerializeField]
    Image unitCost;
    [SerializeField]
    TextMeshProUGUI unitCostTxt;
    PlayerController player;
    [SerializeField]
    Image coolImg;
    [SerializeField] Image fadeImg;

    bool fadeCheck = true;


    UnitNode unitNode;
    GameObject unitObj;
    GameObject unitSpawnBtnPrefab;

    public static Queue<GameObject> gameQueue = new Queue<GameObject>();
    //UI_EventHandler evt;

    // Start is called before the first frame update

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        for(int ii = 0; ii < GlobalData.g_SlotUnitClass.Count; ii++)
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
        ButtonEvent(uiAttackBtn.gameObject, player.AttackWait, UIEvnet.PointerDown);
        ButtonEvent(uiAttackBtn.gameObject, player.ShotArrow, UIEvnet.PointerUp);



        for (int i = 0; i < uiUnit.Length; i++)
        {
            uiUnit[i].TryGetComponent<UnitNode>(out UnitNode node);

            Debug.Log(node.Unit);

            switch (node.Unit)
            {
                case UnitClass.Warrior:
                    UnitButtonSetting(i, "Prefabs/Unit/KnifeUnit");
                    break;

                case UnitClass.Archer:
                    UnitButtonSetting(i, "Prefabs/Unit/BowUnit");
                    break;

                case UnitClass.Spear:
                    Debug.Log("스피어맞음?");
                    UnitButtonSetting(i, "Prefabs/Unit/SpearUnit");
                    break;
            }
        }



        return true;
    }


    private void Update()
    {
        FadeOut();
    }


    void ButtonEvent(GameObject obj,Action action = null, UIEvnet type = UIEvnet.PointerDown)
    {
        UI_EventHandler evt;
        obj.TryGetComponent(out evt);

        switch(type)
        {
            case UIEvnet.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;

             case UIEvnet.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
        }
        
    }

    void ButtonEvent1(GameObject obj, string path,int idx, Action<string,int> action = null, UIEvnet type = UIEvnet.PointerDown)
    {
        UI_EventHandler evt;
        obj.TryGetComponent(out evt);


        switch (type)
        {
            case UIEvnet.PointerDown:
                evt.OnPointerDownUnitHandler -= (unUsedPath,unUsedIdx) => action(path,idx);
                evt.OnPointerDownUnitHandler += (unUsedPath,unUsedIdx) => action(path,idx);
                
                break;

        }

    }


    void UnitButtonSetting(int i , string path)
    {
        ButtonEvent1(uiUnit[i].gameObject, path, i, UnitSummon, UIEvnet.PointerDown);
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

        Debug.Log(cool);

        if (coolImg?.fillAmount > cool)
            coolImg.fillAmount -= Time.deltaTime * speed;

        if (cool >= .99f)
            coolImg.fillAmount = 1;


    }

    public void UpdateUnitCostEnable(Image img,Color grayColor, float unitCost)
    {

        if (Managers.Game.CurCost < unitCost)
        {
            img.color = grayColor;      //매개변수에서 받은 컬러값
        }
        else
            img.color = new Color(1,1,1);
    }

    public void UpdateCost(float curCost)
    {
        unitCostTxt.text = curCost.ToString();

    }



    //유닛 버튼을 누를시 소환
    public void UnitSummon(string namePath,int idx)
    {
        //오브젝트 풀링은 나중에(최적화 작업)
        unitObj = Resources.Load<GameObject>(namePath);
        unitObj.TryGetComponent(out unitNode);

        uiUnit[idx].TryGetComponent(out unitNode);

        if (unitNode?.CoolCheck == false) //쿨타임이 돌고 있지않을 때
        {
            if(Managers.Game.CostCheck(Managers.Game.CurCost, unitNode.UnitCost))
            {
                Managers.Game.CurCost = Managers.Game.CostUse(Managers.Game.CurCost,unitNode.UnitCost);
                UpdateCost(Managers.Game.CurCost);
                gameQueue.Enqueue(unitObj);
                unitNode.CoolCheck = true;          //큐에 넣어주면서 쿨타임 온
            }


        }
    }

    public void Summon(GameObject obj, Transform[] tr)
    {
        obj = gameQueue.Dequeue();
        GameObject instancObj = Instantiate(obj);

        int ran = UnityEngine.Random.Range(0, 3);
        instancObj.transform.position = tr[ran].position;
    }
}
