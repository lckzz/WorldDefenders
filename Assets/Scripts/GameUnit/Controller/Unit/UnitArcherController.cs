using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitArcherController : UnitController
{

    private Transform posTr;
    private readonly string appearDialogSubKey = "archerAppear";


    public override void OnEnable()
    {
        base.OnEnable();
        if (sp != null && myColl != null)
            speechBubble.SpeechBubbuleOn(appearTitleKey, appearDialogSubKey, appearProbability);

    }

    public override void Init()
    {
        base.Init();

        unitStat = Managers.Data.warriorDict[Managers.Game.UnitArcherLv];

        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;

        moveSpeed = 2.5f;
        maxHp = hp;

        posTr = transform.Find("ArrowPos");


        SetUnitState(UnitState.Run);

        speechBubble.SpeechBubbuleOn(appearTitleKey, appearDialogSubKey, appearProbability);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnAttack()
    {
        if (monTarget != null)
        {
            GameObject obj = Managers.Resource.Load<GameObject>("Prefabs/Weapon/UnitArrow");

            if (obj != null)
            {
                Managers.Sound.Play("Sounds/Effect/Bow");
                GameObject arrow = Managers.Resource.Instantiate(obj, posTr.position, Quaternion.identity, this.transform);
                arrow.TryGetComponent(out ArrowCtrl arrowCtrl);
                arrowCtrl.Init();
                //if(monTarget is MonsterController monsterCtrl)
                //{
                //    arrowCtrl.SetType(monsterCtrl, null);

                //}
                //else if(monTarget is EliteMonsterController elite)
                //{
                //    arrowCtrl.SetType(elite, null);

                //}
            }
        }

        else if (monsterPortal != null)
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/UnitArrow");

            if (obj != null)
            {
                Managers.Sound.Play("Sounds/Effect/Bow");
                GameObject arrow = Managers.Resource.Instantiate(obj, posTr.position, Quaternion.identity, this.transform);
                arrow.TryGetComponent(out ArrowCtrl arrowCtrl);
                arrowCtrl.Init();
            }
        }
    }
}
