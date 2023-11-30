using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject 
{
    public void AddObserver(IObserver observer);  //옵저버를 추가하는 함수
    public void RemoveObserver(IObserver observer); //현재 리스트에 들어있는 옵저버를 삭제하는 함수
    public void NotifyToObserver();   //

}
