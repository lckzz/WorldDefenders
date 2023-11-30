using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Debuff : MonoBehaviour
{
    [SerializeField] private GameObject content;    //���Ͱ� ������ �����UI�� ������ ��ġ
    [SerializeField] private GameObject debuffIconUIPrefabs;      //���Ͱ� ������ ����� �����UI ������
    protected GameObject debuffIconUIgo;          //���� ����� UI ���ӿ�����Ʈ

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
            //�̹� �����Ǿ��ִٸ� �������ϰ�
            debuffUIInstantiateisOn = true;
            debuffIconUIgo = Managers.Resource.Instantiate(debuffIconUIPrefabs, content.transform);
            Debug.Log(debuffIconUIgo);
            debuffIconUIgo.TryGetComponent(out debuffUI);
            debuffUI.UIInit(debuffType);

        }
        //����� ������ �����ϰ� ���⼭ �ʱ�ȭ���ش�.
    }





}
