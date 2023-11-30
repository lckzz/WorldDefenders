using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Debuff : MonoBehaviour
{
    [SerializeField] private GameObject content;    //몬스터가 맞으면 디버프UI를 생성할 위치
    [SerializeField] private GameObject debuffIconUIPrefabs;      //몬스터가 맞으면 생기는 디버프UI 프리팹
    protected GameObject debuffIconUIgo;          //몬스터 디버프 UI 게임오브젝트

    private GameObject canvas;
    private GameObject buffScrollViewObj;
    protected GameObject debuffObj;
    protected bool debuffInstantiateisOn = false;
    protected bool debuffUIInstantiateisOn = false;

    protected DebuffUI debuffUI;


    protected virtual void Start()
    {

        if (debuffIconUIPrefabs == null)
            debuffIconUIPrefabs = Managers.Resource.Load<GameObject>("Prefabs/DebuffIcon");

        canvas = this.gameObject.transform.Find("Canvas").gameObject;
        buffScrollViewObj = canvas.transform.Find("BuffScrollView").gameObject;
        content = buffScrollViewObj.transform.Find("Viewport").transform.Find("Content").gameObject;

        debuffObj = transform.Find("Debuff").gameObject;

    }


    public virtual void DebuffOnOff(bool isOn, Unit unit = null) { }

    public virtual void DebuffInstantiate() { }

    public virtual void DebuffIconUIInstantiate(DebuffType debuffType) 
    {
        if(debuffUIInstantiateisOn == false)
        {
            //이미 생성되어있다면 생성못하게
            debuffUIInstantiateisOn = true;
            debuffIconUIgo = Managers.Resource.Instantiate(debuffIconUIPrefabs, content.transform);
            Debug.Log(debuffIconUIgo);
            debuffIconUIgo.TryGetComponent(out debuffUI);
            debuffUI.UIInit(debuffType);

        }
        //디버프 아이콘 생성하고 여기서 초기화해준다.
    }





}
