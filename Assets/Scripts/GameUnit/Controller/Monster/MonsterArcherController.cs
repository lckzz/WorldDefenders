using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterArcherController : MonsterController
{

    //아처 전용
    Transform arrowPos;

    //아처 전용

    private readonly string appearDialogSubKey = "archerAppear";

    public override void OnEnable()
    {
        base.OnEnable();
        if (sp != null && myColl != null)
        {
            speechBubble.SpeechBubbuleOn(appearTitleKey, appearDialogSubKey, appearProbability);
            Init();
        }

    }

    public override void Init()
    {
        base.Init();
        switch (Managers.Game.CurStageType)
        {
            case SubStage.West:
                monStat = Managers.Data.monsterDict[GlobalData.g_BowSkeletonID];
                break;
            case SubStage.East:
                monStat = Managers.Data.monsterDict[GlobalData.g_MidBowSkeletonID];
                break;
            case SubStage.South:
                monStat = Managers.Data.monsterDict[GlobalData.g_HighBowSkeletonID];
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

        arrowPos = transform.Find("ArrowPos");
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
            GameObject obj = Managers.Resource.Load<GameObject>("Prefabs/Weapon/MonsterArrow");

            if (obj != null)
            {
                Managers.Sound.Play("Sounds/Effect/Bow");
                GameObject arrow = Managers.Resource.Instantiate(obj, arrowPos.position, Quaternion.identity, this.transform);
                arrow.TryGetComponent(out MonsterArrowCtrl arrowCtrl);
                arrowCtrl.Init();

            }
        }

        else if (playerTowerCtrl != null)
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/MonsterArrow");

            if (obj != null)
            {
                Managers.Sound.Play("Sounds/Effect/Bow");
                GameObject arrow = Managers.Resource.Instantiate(obj, arrowPos.position, Quaternion.identity, this.transform);
                arrow.TryGetComponent(out MonsterArrowCtrl arrowCtrl);
                arrowCtrl.Init();

            }
        }
    }
}
