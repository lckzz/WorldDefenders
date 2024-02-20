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
        if (myColl != null)
        {
            Init();
            speechBubble.SpeechBubbleOn(appearTitleKey, appearDialogSubKey, appearProbability);

        }

    }

    public override void Init()
    {
        base.Init();

        SetMonsterState(MonsterState.Run);
        speechBubble.SpeechBubbleOn(appearTitleKey, appearDialogSubKey, appearProbability);
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
