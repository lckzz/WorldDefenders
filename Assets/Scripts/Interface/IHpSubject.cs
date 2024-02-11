using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHpSubject
{
    public void AddHpObserver(IHpObserver observer);  //�������� �߰��ϴ� �Լ�
    public void RemoveHpObserver(IHpObserver observer); //���� ����Ʈ�� ����ִ� �������� �����ϴ� �Լ�
    public void NotifyToHpObserver();   //
}
