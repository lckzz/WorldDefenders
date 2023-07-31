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
    //�ΰ����� UI���� ��ɵ�
    [SerializeField]
    Button uiUnitSword;         //���߿� ������ ������ ���ֵ��� ������ �޾Ƽ� ��ư�迭�� ó���ؾ���
    [SerializeField]
    Button uiUnitBow;
    [SerializeField]
    Button uiAttackBtn;

    [SerializeField]
    GameObject contents;

    Button[] uiUnit;            //������ ���ֵ��� ��ư����

    [SerializeField]
    Image unitCost;
    [SerializeField]
    TextMeshProUGUI unitCostTxt;
    PlayerController player;
    [SerializeField]
    Image coolImg;


    UnitNode unitNode;
    GameObject unitObj;

    public static Queue<GameObject> gameQueue = new Queue<GameObject>();
    //UI_EventHandler evt;

    // Start is called before the first frame update

    public override bool Init()
    {
        if (base.Init() == false)
            return false;



       
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
            if(i == 0)
            {
                string path = "Prefab/Unit/KnifeUnit";
                ButtonEvent1(uiUnit[i].gameObject, path, i,UnitSummon, UIEvnet.PointerDown);
            }
            if (i == 1)
            {
                string path = "Prefab/Unit/BowUnit";
                ButtonEvent1(uiUnit[i].gameObject, path, i,UnitSummon, UIEvnet.PointerDown);
            }
        }



        return true;
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

    public void UpdateCost(float curCost, float maxCost)
    {
        if (unitCost.fillAmount >= .0f && unitCost.fillAmount < 1.0f)
            unitCost.fillAmount = curCost / maxCost;

        if (unitCost.fillAmount >= 1.0f)
            unitCost.fillAmount = 1.0f;

        unitCostTxt.text = curCost + " / " + maxCost;

    }



    //���� ��ư�� ������ ��ȯ
    public void UnitSummon(string namePath,int idx)
    {
        //������Ʈ Ǯ���� ���߿�(����ȭ �۾�)
        unitObj = Resources.Load<GameObject>(namePath);
        unitObj.TryGetComponent(out unitNode);

        uiUnit[idx].TryGetComponent(out unitNode);

        if (unitNode?.CoolCheck == false) //��Ÿ���� ���� �������� ��
        {
            if(Managers.Game.CostCheck(Managers.Game.CurCost, unitNode.UnitCost))
            {
                Managers.Game.CurCost = Managers.Game.CostUse(Managers.Game.CurCost,unitNode.UnitCost);
                UpdateCost(Managers.Game.CurCost, Managers.Game.MaxCost);
                gameQueue.Enqueue(unitObj);
                unitNode.CoolCheck = true;          //ť�� �־��ָ鼭 ��Ÿ�� ��
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
