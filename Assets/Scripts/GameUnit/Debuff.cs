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
        //약화스킬을 위한 유닛 정보셋팅
        unitSpeed = speed;
        unitAtt = att;
    }

    public IEnumerator StartDebuff(float debuffIdx, float durationTime, Action<bool> debuffOnOffAction)
    {
        WaitForSeconds wfs = new WaitForSeconds(durationTime);

        //기본 속도랑 공격력을 저장하고
        float defalutMoveSpeed = unitSpeed;
        int defalutAtt = unitAtt;

        //디버프당하면 속도랑 공격력이 낮아짐
        unitSpeed -= unitSpeed * (debuffIdx / 100);
        unitAtt -= (int)(unitAtt * (debuffIdx / 100));

        yield return wfs;       //시간초만큼 대기하고 다시 원래대로 돌려줌

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
        if (fireDebuffObj.activeSelf)    //화상디버프가 켜진다면
            FireDebuff(unit);       //지속데미지 디버프 시작
    }

    public void FireDebuff(Unit unit)
    {
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);

        fireCoroutine = StartCoroutine(FireDebuffCo(unit));
    }

    IEnumerator FireDebuffCo(Unit unit)  //화상디버프 지속데미지
    {
        int damageCount = 0;  //초당 늘어남
        int maxTime = 10;

        float durationTime = 1.0f;

        WaitForSeconds wfs = new WaitForSeconds(durationTime);      //초당 지속데미지줄것
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
