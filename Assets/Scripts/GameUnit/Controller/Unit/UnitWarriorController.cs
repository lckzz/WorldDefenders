using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWarriorController : UnitController
{

    private readonly string warriorHitSound = "WarriorAttack";
    private readonly string warriorCriticalSound = "CriticalSound";
    private readonly string warriorHitEff = "HitEff";

    private readonly string appearDialogSubKey = "warriorAppear";




    public override void OnEnable()
    {
        base.OnEnable();
        if (sp != null && myColl != null)
            speechBubble.SpeechBubbuleOn(appearTitleKey,appearDialogSubKey, appearProbability);

    }

    public override void Init()
    {
        base.Init();

        unitStat = Managers.Data.warriorDict[Managers.Game.UnitWarriorLv];

        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;

        moveSpeed = 2.5f;
        maxHp = hp;

        SetUnitState(UnitState.Run);

        speechBubble.SpeechBubbuleOn(appearTitleKey, appearDialogSubKey, appearProbability);


    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }



    public override void OnAttack()
    {
        if (monTarget != null)
        {
            float dist = (monTarget.transform.position - this.gameObject.transform.position).magnitude;
            if (dist < unitStat.attackRange + 0.5f)
                CriticalAttack(monTarget, warriorHitSound, warriorCriticalSound, warriorHitEff);
        }
        else if (monsterPortal != null)
            CriticalAttack(monsterPortal, warriorHitSound, warriorCriticalSound, warriorHitEff);
    }
}
