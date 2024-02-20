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
        Managers.UI.ShowPopUp<UI_GameResult>().SetStageType(stageStateType);     //���Ӱ��â �˾��� ������
    }

    public void StageInfoInit()
    {
        if (Managers.Game.OneChapterStageInfoList.Count > 0)
            return;     //���� �̹� ��ü�� �����Ǿ��ִٸ� ����

        int ii = 0;
        foreach(var data in GetStageData())     //��ųʸ��� ������ŭ ��ü ����
        {
            OneChapterStageInfo stageInfo = null;

            if (ii == 0)   //ó������ ����α�
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

        //���� ���ӻ��°� �÷������� �ƴ϶�� Ʈ��
        if (stageStateType != Define.StageStageType.Playing)
            return true;

        return false;
    }

}
