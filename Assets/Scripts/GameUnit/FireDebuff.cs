using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FireDebuff : Debuff
{

    private Coroutine fireCoroutine = null;
    private GameObject fireDebuffPrefab;
    private GameObject fireDebuffGo;

    protected override void Start()
    {
        base.Start();
        fireDebuffPrefab = Managers.Resource.Load<GameObject>("Prefabs/FireEffect");
    }

    public override void DebuffInstantiate()
    {
        if(debuffInstantiateisOn == false)
        {
            debuffInstantiateisOn = true;
            fireDebuffGo = Managers.Resource.Instantiate(fireDebuffPrefab, debuffObj.transform);

        }
    }

    public override void DebuffOnOff(bool isOn, Unit unit = null)
    {
        if (fireDebuffGo == null || debuffUI == null)
            return;


        if(isOn == true)
            DebuffInstantiate();

        fireDebuffGo.SetActive(isOn);
        if (fireDebuffGo.activeSelf)    //화상디버프가 켜진다면
            FireDebuffOn(unit);       //지속데미지 디버프 시작
        else //화상디버프가 꺼진다면
            debuffUI?.DebuffUIDestroy();     //디버프UI가 있다면 삭제
    }

    public void FireDebuffOn(Unit unit)
    {
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);

        fireCoroutine = StartCoroutine(FireDebuffCo(unit));
    }


    IEnumerator FireDebuffCo(Unit unit)  //화상디버프 지속데미지
    {
        DebuffIconUIInstantiate(DebuffType.Fire);      //디버프 UI 생성

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
                DebuffOnOff(false);
                debuffUIInstantiateisOn = false;        //
                debuffUI.DebuffUIDestroy();
                yield break;

            }
        }
    }

}
