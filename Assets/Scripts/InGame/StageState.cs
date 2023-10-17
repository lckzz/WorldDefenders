using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageState 
{
    private Define.StageStageType stageStateType = Define.StageStageType.Playing;
    private Define.SubStage curStageType = Define.SubStage.East;

    public Define.SubStage CurStageType { get { return curStageType; } set { curStageType = value; } }

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


    public bool GameEndResult()
    {

        //현재 게임상태가 플레이중이 아니라면 트루
        if (stageStateType != Define.StageStageType.Playing)
            return true;

        return false;
    }

}
