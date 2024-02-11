using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpearController : UnitController
{

    private readonly string hitSound = "WarriorAttack";
    private readonly string criticalSound = "CriticalSound";
    private readonly string hitEff = "HitEff";

    private readonly string appearDialogSubKey = "spearAppear";


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

        unitStat = Managers.Data.spearDict[Managers.Game.UnitSpearLv];
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
                CriticalAttack(monTarget, hitSound, criticalSound, hitEff);
        }

        else if (monsterPortal != null)
            CriticalAttack(monsterPortal, hitSound, criticalSound, hitEff);
    }
}
