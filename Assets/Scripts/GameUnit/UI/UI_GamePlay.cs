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
    [SerializeField]
    Image coolImg;
    [SerializeField] Image fadeImg;

    bool fadeCheck = true;
    float uiAttBtnBeforePosY = -450.0f;
    float uiAttBtnAfterPosY = -280.0f;

    float uiCostBeforePosY = -60.0f;
    float uiCostAfterPosY = 70.0f;

    UnitNode unitNode;
    GameObject unitObj;
    GameObject unitSpawnBtnPrefab;

    [SerializeField] private Vector3[] unitNodePos;
    Vector3 unitNodeStartPos = new Vector3(105.0f, -260.0f, .0f);



    public static Queue<GameObject> gameQueue = new Queue<GameObject>();
    //UI_EventHandler evt;

    // Start is called before the first frame update

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


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
        ButtonEvent(uiAttackBtn.gameObject, player.AttackWait, UIEvent.PointerDown);
        ButtonEvent(uiAttackBtn.gameObject, player.ShotArrow, UIEvent.PointerUp);



        for (int i = 0; i < uiUnit.Length; i++)
        {
            uiUnit[i].TryGetComponent<UnitNode>(out UnitNode node);


            switch (node.Unit)
            {
                case UnitClass.Warrior:
                    if(GlobalData.g_UnitWarriorLv < 5)
                        UnitButtonSetting(i, "Prefabs/Unit/Warrior/WarriorUnitLv1");
                    else if(GlobalData.g_UnitWarriorLv >= 5)
                        UnitButtonSetting(i, "Prefabs/Unit/Warrior/WarriorUnitLv2");
                    break;

                case UnitClass.Archer:
                    if(GlobalData.g_UnitArcherLv < 5)
                        UnitButtonSetting(i, "Prefabs/Unit/Archer/ArcherUnitLv1");
                    else
                        UnitButtonSetting(i, "Prefabs/Unit/Archer/ArcherUnitLv2");

                    break;

                case UnitClass.Spear:
                    if(GlobalData.g_UnitSpearLv < 5)
                        UnitButtonSetting(i, "Prefabs/Unit/Spear/SpearUnitLv1");
                    else
                        UnitButtonSetting(i, "Prefabs/Unit/Spear/SpearUnitLv2");

                    break;
                case UnitClass.Magician:
                    UnitButtonSetting(i, "Prefabs/Unit/MagicianUnit");
                    break;
            }
        }

        StartUISet = StartCoroutine(StartUnitNodeUIMove());


        return true;
    }

    Coroutine StartUISet = null;

    private void Update()
    {
        FadeOut();
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
        {
            UiMove(rt, uiCostBeforePosY, uiCostAfterPosY);
            yield return wfs;
        }

        yield break;
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
        ButtonEvent1(uiUnit[i].gameObject, path, i, UnitSummon, UIEvent.PointerDown);
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


        if (coolImg?.fillAmount > cool)
            coolImg.fillAmount -= Time.deltaTime * speed;

        if (cool >= .99f)
            coolImg.fillAmount = 1;


    }

    public void UpdateUnitCostEnable(Image img,Color grayColor, float unitCost)
    {

        if (GameManager.instance.CurCost < unitCost)
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
            if(GameManager.instance.CostCheck(GameManager.instance.CurCost, unitNode.UnitCost))
            {
                GameManager.instance.CurCost = GameManager.instance.CostUse(GameManager.instance.CurCost,unitNode.UnitCost);
                UpdateCost(GameManager.instance.CurCost);
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

    public void UiMove(RectTransform rt, float beforePosY , float afterPosY)
    {
        if (rt == null)
            return;


        if (rt.anchoredPosition.y == beforePosY)
            rt.DOLocalMoveY(afterPosY, 0.5f).SetEase(Ease.OutBack);
    }
}
