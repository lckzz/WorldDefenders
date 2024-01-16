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
        weaknessDebuffPrefab = Managers.Resource.Load<GameObject>("Prefabs/WeaknessEffect");  //�������� �޾ƿ´�.
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
        //��ȭ��ų�� ���� ���� ��������
        unitSpeed = speed;
        unitAtt = att;
    }

    public IEnumerator StartWeaknessDebuff(float debuffIdx, float durationTime)
    {

        DebuffIconUIInstantiate(DebuffType.Weakness);
        WaitForSeconds wfs = new WaitForSeconds(durationTime);

        //�⺻ �ӵ��� ���ݷ��� �����ϰ�
        originalUnitSpeed = unitSpeed;
        originalUnitAtt = unitAtt;

        //��������ϸ� �ӵ��� ���ݷ��� ������
        unitSpeed -= unitSpeed * (debuffIdx / 100);
        unitAtt -= (int)(unitAtt * (debuffIdx / 100));

        NotifyToObserver();     //��ȭ�� ������ �������鿡�� ������

        yield return wfs;       //�ð��ʸ�ŭ ����ϰ� �ٽ� ������� ������

        unitSpeed = originalUnitSpeed;
        unitAtt = originalUnitAtt;

        NotifyToObserver();     //��ȭ�� ������ �������鿡�� ������


        DebuffOnOff(false);

        yield return null;

    }

    public void UnitDebuff(float debuffIdx, float durationTime)
    {
        courtine = StartCoroutine(StartWeaknessDebuff(debuffIdx, durationTime));
    }

    public override void DebuffOnOff(bool isOn,Unit unit = null) 
    {
        Debug.Log("���⿡�Ȥ�������������" + weaknessDebuffGo);
        Debug.Log("���⿡�Ȥ�������������2222" + debuffUI);


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



    //--------������ ����(��ü�� �������� �����ϰų� ���������� ������ �ѷ����ִ� ����)
    //������� ����
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
        //���Ͱ� ������ ����� �ʱ�ȭ
        base.DebuffDestory();
        DebuffInit();
    }


    void DebuffInit()
    {
        if (courtine != null)
            StopCoroutine(courtine);

        unitSpeed = originalUnitSpeed;
        unitAtt = originalUnitAtt;
        NotifyToObserver();     //��ȭ�� ������ �������鿡�� ������ (��������·� ������ �������� �����ϰ� �ٽ� �������� ��������)

    }


    //private void OnDisable()
    //{
    //    DebuffInit();
    //}


}
