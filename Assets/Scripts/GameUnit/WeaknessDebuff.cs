using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WeaknessDebuff : Debuff, ISubject
{
    private List<IObserver> observers = new List<IObserver>();

    private float unitSpeed = 0.0f;
    private int unitAtt = 0;
    private GameObject weaknessDebuffPrefab;
    private GameObject weaknessDebuffGo;

    private float originalUnitSpeed;
    private int originalUnitAtt;


    protected override void Start()
    {
        base.Start();
        weaknessDebuffPrefab = Managers.Resource.Load<GameObject>("Prefabs/WeaknessEffect");  //覗軒噸聖 閤焼紳陥.
    }

    public override void DebuffInstantiate()
    {

        if (debuffInstantiateisOn == false)
        {
            debuffInstantiateisOn = true;
            weaknessDebuffGo = Managers.Resource.Instantiate(weaknessDebuffPrefab, debuffObj.transform);

        }
    }

    public void WeaknessSkillInfo(float speed, int att)
    {
        //鉦鉢什迭聖 是廃 政間 舛左実特
        unitSpeed = speed;
        unitAtt = att;
    }

    public IEnumerator StartWeaknessDebuff(float debuffIdx, float durationTime)
    {

        DebuffIconUIInstantiate(DebuffType.Weakness);
        WaitForSeconds wfs = new WaitForSeconds(durationTime);

        //奄沙 紗亀櫛 因維径聖 煽舌馬壱
        originalUnitSpeed = unitSpeed;
        originalUnitAtt = unitAtt;

        //巨獄覗雁馬檎 紗亀櫛 因維径戚 碍焼像
        unitSpeed -= unitSpeed * (debuffIdx / 100);
        unitAtt -= (int)(unitAtt * (debuffIdx / 100));

        NotifyToObserver();     //痕鉢吉 葵級聖 辛煽獄級拭惟 穿背捜

        yield return wfs;       //獣娃段幻鏑 企奄馬壱 陥獣 据掘企稽 宜形捜

        unitSpeed = originalUnitSpeed;
        unitAtt = originalUnitAtt;

        NotifyToObserver();     //痕鉢吉 葵級聖 辛煽獄級拭惟 穿背捜


        DebuffOnOff(false);

        yield return null;

    }

    public void UnitDebuff(float debuffIdx, float durationTime)
    {
        courtine = StartCoroutine(StartWeaknessDebuff(debuffIdx, durationTime));
    }

    public override void DebuffOnOff(bool isOn,Unit unit = null) 
    {
        Debug.Log("食奄拭身いしいけしいけ" + weaknessDebuffGo);
        Debug.Log("食奄拭身いしいけしいけ2222" + debuffUI);


        if (debuffUI == null)
            return;



        if (isOn == true)
            DebuffInstantiate();
        else
        {
            Managers.Resource.Destroy(weaknessDebuffGo);
            debuffUI?.DebuffUIDestroy();

        }
    }



    //--------辛煽獄 鳶渡(爽端拭 辛煽獄亜 姥偽馬暗蟹 辛煽獄拭惟 舛左研 姿形澗爽澗 蝕拝)
    //巨獄覗研 閤精
    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        if (observers.IndexOf(observer) > 0)
            observers.Remove(observer);
    }

    public void NotifyToObserver()
    {
        foreach (IObserver ops in observers)
            ops.Notified(unitAtt, unitSpeed);
    }

    public override void DebuffDestory()
    {
        //佼什斗亜 宋生檎 巨獄覗 段奄鉢
        base.DebuffDestory();
        DebuffInit();
    }


    void DebuffInit()
    {
        if (courtine != null)
            StopCoroutine(courtine);

        unitSpeed = originalUnitSpeed;
        unitAtt = originalUnitAtt;
        NotifyToObserver();     //痕鉢吉 葵級聖 辛煽獄級拭惟 穿背捜 (巨獄覗雌殿稽 宋生檎 据掘葵聖 旋遂馬壱 陥獣 佼什斗廃砺 穿含背捜)

    }


    //private void OnDisable()
    //{
    //    DebuffInit();
    //}


}
