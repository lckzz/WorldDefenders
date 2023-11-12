using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    [SerializeField] private GameObject debuffObj;
    [SerializeField] private GameObject fireDebuffObj;
    private Coroutine fireCoroutine = null;



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
