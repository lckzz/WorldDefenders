using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    [SerializeField] private GameObject debuffObj;
    [SerializeField] private GameObject fireDebuffObj;
    [SerializeField] private GameObject debuffIconPrefabs;
    private Coroutine fireCoroutine = null;

    private float unitSpeed = 0.0f;
    private int unitAtt = 0;


    private void Start()
    {
        if (debuffIconPrefabs == null)
            debuffIconPrefabs = Managers.Resource.Load<GameObject>("Prefabs/DebuffIcon");
    }

    public void WeaknessSkillInfo(float speed, int att)
    {
        //��ȭ��ų�� ���� ���� ��������
        unitSpeed = speed;
        unitAtt = att;
    }

    public IEnumerator StartDebuff(float debuffIdx, float durationTime, Action<bool> debuffOnOffAction)
    {
        WaitForSeconds wfs = new WaitForSeconds(durationTime);

        //�⺻ �ӵ��� ���ݷ��� �����ϰ�
        float defalutMoveSpeed = unitSpeed;
        int defalutAtt = unitAtt;

        //��������ϸ� �ӵ��� ���ݷ��� ������
        unitSpeed -= unitSpeed * (debuffIdx / 100);
        unitAtt -= (int)(unitAtt * (debuffIdx / 100));

        yield return wfs;       //�ð��ʸ�ŭ ����ϰ� �ٽ� ������� ������

        unitSpeed = defalutMoveSpeed;
        unitAtt = defalutAtt;

        debuffOnOffAction(false);

        yield return null;

    }

    public void UnitDebuff(float debuffIdx, float durationTime, Action<bool> debuffOnOffAction)
    {
        StartCoroutine(StartDebuff(debuffIdx, durationTime, debuffOnOffAction));
    }

    public void DebuffOnOff(bool isOn)
    {
        debuffObj.SetActive(isOn);

   
    }

    public void FireDebuffOnOff(bool isOn, Unit unit = null)
    {
        fireDebuffObj.SetActive(isOn);
        if (fireDebuffObj.activeSelf)    //ȭ�������� �����ٸ�
            FireDebuff(unit);       //���ӵ����� ����� ����
    }

    public void FireDebuff(Unit unit)
    {
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);

        fireCoroutine = StartCoroutine(FireDebuffCo(unit));
    }

    IEnumerator FireDebuffCo(Unit unit)  //ȭ������ ���ӵ�����
    {
        int damageCount = 0;  //�ʴ� �þ
        int maxTime = 10;

        float durationTime = 1.0f;

        WaitForSeconds wfs = new WaitForSeconds(durationTime);      //�ʴ� ���ӵ������ٰ�
        while (true)
        {
            yield return wfs;

            Debug.Log(unit);
            unit.OnDamage(10);
            damageCount++;

            if (damageCount >= maxTime)
            {
                FireDebuffOnOff(false);
                yield break;

            }
        }
    }
}
