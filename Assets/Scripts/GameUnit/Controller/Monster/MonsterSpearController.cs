using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterSpearController : MonsterController
{
    private readonly string spearHitSound = "WarriorAttack";
    private readonly string spearCriticalSound = "CriticalSound";
    private readonly string spearHitEff = "HitEff";

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
            float dist = (unitTarget.transform.position - this.gameObject.transform.position).magnitude;
            if (dist < monStat.attackRange)
                CriticalAttack(unitTarget, spearHitSound, spearCriticalSound, spearHitEff);

            Debug.Log(dist);
            Debug.Log(monStat.attackRange);
        }


        else if (playerTowerCtrl != null)
            CriticalAttack(playerTowerCtrl, spearHitSound, spearCriticalSound, spearHitEff);
    }
}
