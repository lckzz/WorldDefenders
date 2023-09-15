using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitInfoSelectPopUp : UI_Base
{

    //캐릭터 셋팅에서 선택한 유닛의 정보를 볼수 있고 배치할 수 있는 버튼이 있는 곳이다.
    //여기서 연결해야 되는 부분들은 일단 내가 선택한 유닛의 정보를 가지고 와야한다.
    //가지고 온 유닛의 데이터를 받아와서 UI(이름, 공격력,체력,레벨, 캐릭터 생성, 설명) 텍스트를 연결하고
    //배치버튼을 누르면 팝업이 꺼지고 해당 캐릭터의 정보를 선택창에 보내준다.
    //스킬은 따로 스킬인포를 만들어주고 유닛 타입을 만들어서 기본 유닛이면 스페셜스킬오브젝트를 꺼주고 온오프로 관리
    //스킬 인포스크립트를 이용해서 해당 스킬이미지를 누르게 되면 툴팁이 나오게 설정

    public override bool Init()
    {
        base.Init();


        return true;
    }



}
