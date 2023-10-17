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
        Managers.UI.ShowPopUp<UI_GameResult>().SetStageType(stageStateType);     //���Ӱ��â �˾��� ������
    }


    public bool GameEndResult()
    {

        //���� ���ӻ��°� �÷������� �ƴ϶�� Ʈ��
        if (stageStateType != Define.StageStageType.Playing)
            return true;

        return false;
    }

}
