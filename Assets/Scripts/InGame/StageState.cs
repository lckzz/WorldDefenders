using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageState 
{
    private Define.StageStageType stageStateType = Define.StageStageType.Playing;
    private Define.Stage curStageType = Define.Stage.South;

    public Define.Stage CurStageType { get { return curStageType; } set { curStageType = value; } }

    public Define.StageStageType StageStateType 
    { 
        get { return stageStateType; }
        set {  stageStateType = value; } 
    }



    public void ResultState(Define.StageStageType type)
    {
        stageStateType = type;
        Managers.UI.ShowPopUp<UI_GameResult>().SetStageType(stageStateType);     //게임결과창 팝업을 열어줌
    }

    public void StageInfoInit()
    {
        if (Managers.Game.OneChapterStageInfoList.Count > 0)
            return;     //만약 이미 객체가 생성되어있다면 리턴

        int ii = 0;
        foreach(var data in GetStageData())     //딕셔너리의 개수만큼 객체 생성
        {
            OneChapterStageInfo stageInfo = null;

            if (ii == 0)   //처음꺼는 열어두기
                stageInfo = new OneChapterStageInfo(data,data.id,false,1);
            else
                stageInfo = new OneChapterStageInfo(data,data.id);

            Managers.Game.OneChapterStageInfoList.Add(stageInfo);

            ii++;
        }

        Debug.Log(Managers.Game.OneChapterStageInfoList[3].StageData.name);
    }



    public IEnumerable<StageData> GetStageData()
    {
        return Managers.Data.stageDict.Values;
    }


    public bool GameEndResult()
    {

        //현재 게임상태가 플레이중이 아니라면 트루
        if (stageStateType != Define.StageStageType.Playing)
            return true;

        return false;
    }

}
