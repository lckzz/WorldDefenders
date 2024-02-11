using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHpSubject
{
    public void AddHpObserver(IHpObserver observer);  //옵저버를 추가하는 함수
    public void RemoveHpObserver(IHpObserver observer); //현재 리스트에 들어있는 옵저버를 삭제하는 함수
    public void NotifyToHpObserver();   //
}
