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
        weaknessDebuffPrefab = Managers.Resource.Load<GameObject>("Prefabs/WeaknessEffect");  //프리팹을 받아온다.
    }

    public override void DebuffInstantiate()
    {

        if (debuffInstantiateisOn == false)
        {
            Debug.Log("여기 디버프생성");
            debuffInstantiateisOn = true;
            weaknessDebuffGo = Managers.Resource.Instantiate(weaknessDebuffPrefab, debuffObj.transform);

        }
    }

    public void WeaknessSkillInfo(float speed, int att)
    {
        //약화스킬을 위한 유닛 정보셋팅
        unitSpeed = speed;
        unitAtt = att;
    }

    public IEnumerator StartWeaknessDebuff(float debuffIdx, float durationTime)
    {

        DebuffIconUIInstantiate(DebuffType.Weakness);
        WaitForSeconds wfs = new WaitForSeconds(durationTime);

        //기본 속도랑 공격력을 저장하고
        originalUnitSpeed = unitSpeed;
        originalUnitAtt = unitAtt;

        //디버프당하면 속도랑 공격력이 낮아짐
        //unitSpeed -= unitSpeed * (debuffIdx / 100);
        //unitAtt -= (int)(unitAtt * (debuffIdx / 100));
        float floatidx = (unitAtt * (debuffIdx / 100));     //몬스터공격력 75값을 구함
        unitAtt -= Mathf.RoundToInt(floatidx);

        floatidx = (unitSpeed * (debuffIdx / 100));     //몬스터속도 75값을 구함
        unitSpeed -= Mathf.RoundToInt(floatidx);

        Debug.Log("디버프" + debuffIdx / 100);
        Debug.Log("디버프공격" + unitAtt);

        NotifyToObserver();     //변화된 값들을 옵저버들에게 전해줌

        yield return wfs;       //시간초만큼 대기하고 다시 원래대로 돌려줌

        unitSpeed = originalUnitSpeed;
        unitAtt = originalUnitAtt;

        NotifyToObserver();     //변화된 값들을 옵저버들에게 전해줌


        DebuffOnOff(false);

        yield return null;

    }

    public void UnitDebuff(float debuffIdx, float durationTime)
    {
        coroutine = StartCoroutine(StartWeaknessDebuff(debuffIdx, durationTime));
    }

    public override void DebuffOnOff(bool isOn,Unit unit = null) 
    {
        Debug.Log("여기에옴ㄴㅇㄴㅁㅇㄴㅁ" + weaknessDebuffGo);
        Debug.Log("여기에옴ㄴㅇㄴㅁㅇㄴㅁ2222" + debuffUI);
        Debug.Log("인스탄트" + debuffInstantiateisOn);


        if (debuffUI == null)
            return;



        if (isOn == true)
            DebuffInstantiate();
        else
        {
            Debug.Log("여기 디버프 다사라짐" );

            Managers.Resource.Destroy(weaknessDebuffGo);
            debuffUI?.DebuffUIDestroy();
            debuffInstantiateisOn = false;
            debuffUIInstantiateisOn = false;

        }
    }



    //--------옵저버 패턴(주체에 옵저버가 구독하거나 옵저버에게 정보를 뿌려는주는 역할)
    //디버프를 받은
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
        //몬스터가 죽으면 디버프 초기화
        base.DebuffDestory();
        DebuffInit();
    }


    public override void DebuffInit()
    {
        base.DebuffInit();

        debuffInstantiateisOn = false;
        unitSpeed = originalUnitSpeed;
        unitAtt = originalUnitAtt;
        NotifyToObserver();     //변화된 값들을 옵저버들에게 전해줌 (디버프상태로 죽으면 원래값을 적용하고 다시 몬스터한테 전달해줌)

    }


    //private void OnDisable()
    //{
    //    DebuffInit();
    //}


}
