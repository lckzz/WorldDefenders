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
        if (myColl != null)
            speechBubble.SpeechBubbleOn(appearTitleKey,appearDialogSubKey, appearProbability);

    }

    public override void Init()
    {
        base.Init();

        SetUnitState(UnitState.Run);

        speechBubble.SpeechBubbleOn(appearTitleKey, appearDialogSubKey, appearProbability);


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
