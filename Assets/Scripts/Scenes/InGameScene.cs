using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{
    private void Start()
    {
        Init();
    }


    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.BattleStage_Field;
    }

    public override void Clear()
    {

    }
}
