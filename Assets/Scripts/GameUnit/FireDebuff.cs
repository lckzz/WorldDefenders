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
        if (fireDebuffGo.activeSelf)    //ȭ�������� �����ٸ�
            FireDebuffOn(unit);       //���ӵ����� ����� ����
        else //ȭ�������� �����ٸ�
            debuffUI?.DebuffUIDestroy();     //�����UI�� �ִٸ� ����
    }

    public void FireDebuffOn(Unit unit)
    {
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);

        fireCoroutine = StartCoroutine(FireDebuffCo(unit));
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
