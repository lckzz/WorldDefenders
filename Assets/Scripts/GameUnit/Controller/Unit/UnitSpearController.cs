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
        if (myColl != null)
        {
            Init();
            SpeechBubbleOn(appearTitleKey, appearDialogSubKey, appearProbability);

        }

    }

    public override void Init()
    {
        base.Init();

        SetUnitState(UnitState.Run);

        SpeechBubbleOn(appearTitleKey, appearDialogSubKey, appearProbability);
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

            if (dist < unitStat.attackRange)
                CriticalAttack(monTarget, hitSound, criticalSound, hitEff);
        }

        else if (monsterPortal != null)
            CriticalAttack(monsterPortal, hitSound, criticalSound, hitEff);
    }
}
