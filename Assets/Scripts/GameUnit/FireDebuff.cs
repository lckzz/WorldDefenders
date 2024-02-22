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
                debuffUI?.DebuffUIDestroy();     //�����UI�� �ִٸ� ����

            }

        }

    }

    public override void DebuffDestory()
    {
        //���Ͱ� ������ ����� �ʱ�ȭ
        base.DebuffDestory();
        DebuffInit();
    }

    public void FireDebuffOn(Unit unit)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(FireDebuffCo(unit));
    }


    IEnumerator FireDebuffCo(Unit unit)  //ȭ������ ���ӵ�����
    {
        DebuffIconUIInstantiate(DebuffType.Fire);      //����� UI ����

        int damageCount = 0;  //�ʴ� �þ
        int maxTime = 10;

        float durationTime = 1.0f;

        WaitForSeconds wfs = new WaitForSeconds(durationTime);      //�ʴ� ���ӵ������ٰ�
        while (true)
        {
            yield return wfs;

            unit.OnDamage(10);      //10�������� ���ӵ�����
            damageCount++;

            if (damageCount >= maxTime)         //���ӽð��� ������
            {
                DebuffOnOff(false);                 //������� ���ְ�
                debuffUIInstantiateisOn = false;        //�����UI�� �����Ҽ��ְ��ϰ�
                debuffUI.DebuffUIDestroy();         //�����UI�� �����Ѵ�.
                yield break;

            }
        }
    }

}
