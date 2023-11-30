using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject 
{
    public void AddObserver(IObserver observer);  //�������� �߰��ϴ� �Լ�
    public void RemoveObserver(IObserver observer); //���� ����Ʈ�� ����ִ� �������� �����ϴ� �Լ�
    public void NotifyToObserver();   //

}
