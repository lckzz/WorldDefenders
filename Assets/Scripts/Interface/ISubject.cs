using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject 
{
    public void AddObserver(IHpObserver observer);  //�������� �߰��ϴ� �Լ�
    public void RemoveObserver(IHpObserver observer); //���� ����Ʈ�� ����ִ� �������� �����ϴ� �Լ�
    public void NotifyToObserver(IHpObserver observer);   //

}
