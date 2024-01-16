using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Define;

public class MonsterWarriorController : MonsterController
{

    private readonly string warriorHitSound = "WarriorAttack";
    private readonly string warriorCriticalSound = "CriticalSound";
    private readonly string warriorHitEff = "HitEff";

    private readonly string appearDialogSubKey = "warriorAppear";

    public override void OnEnable()
    {
        base.OnEnable();
        if (sp != null && myColl != null)
        {
            Init();
            speechBubble.SpeechBubbuleOn(appearTitleKey, appearDialogSubKey, appearProbability);

        }

    }

    public override void Init()
    {
        base.Init();

        switch (Managers.Game.CurStageType)
        {
            case SubStage.West:
                monStat = Managers.Data.monsterDict[GlobalData.g_NormalSkeletonID];
                break;
            case SubStage.East:
                monStat = Managers.Data.monsterDict[GlobalData.g_MidSkeletonID];
                break;
            case SubStage.South:
                monStat = Managers.Data.monsterDict[GlobalData.g_HighSkeletonID];
                break;
        }


        att = monStat.att;
        hp = monStat.hp;
        maxHp = hp;
        knockbackForce = monStat.knockBackForce;
        attackRange = monStat.attackRange;
        moveSpeed = 2.0f;
        DropGold = monStat.dropGold;
        DropCost = monStat.dropCost;

        SetMonsterState(MonsterState.Run);
        speechBubble.SpeechBubbuleOn(appearTitleKey, appearDialogSubKey, appearProbability);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    public override void OnAttack()
    {

        if (unitTarget != null)
        {
            float dist = (unitTarget.transform.position - this.gameObject.transform.position).sqrMagnitude;
            if (dist < monStat.attackRange * monStat.attackRange)
                CriticalAttack(unitTarget, warriorHitSound, warriorCriticalSound, warriorHitEff);
        }


        else if (playerTowerCtrl != null)
            CriticalAttack(playerTowerCtrl, warriorHitSound, warriorCriticalSound, warriorHitEff);
    }
}
