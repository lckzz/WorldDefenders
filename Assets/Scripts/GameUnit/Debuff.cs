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
