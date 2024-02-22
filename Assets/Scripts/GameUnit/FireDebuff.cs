using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FireDebuff : Debuff
{


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
        if(isOn == true)
            DebuffInstantiate();
        else
        {
            if(fireDebuffGo != null)
            {
                Managers.Resource.Destroy(fireDebuffGo);
                debuffUI?.DebuffUIDestroy();     //디버프UI가 있다면 삭제

            }

        }

    }

    public override void DebuffDestory()
    {
        //몬스터가 죽으면 디버프 초기화
        base.DebuffDestory();
        DebuffInit();
    }

    public void FireDebuffOn(Unit unit)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(FireDebuffCo(unit));
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

            unit.OnDamage(10);      //10데미지의 지속데미지
            damageCount++;

            if (damageCount >= maxTime)         //지속시간이 지나면
            {
                DebuffOnOff(false);                 //디버프를 꺼주고
                debuffUIInstantiateisOn = false;        //디버프UI를 생성할수있게하고
                debuffUI.DebuffUIDestroy();         //디버프UI를 삭제한다.
                yield break;

            }
        }
    }

}
