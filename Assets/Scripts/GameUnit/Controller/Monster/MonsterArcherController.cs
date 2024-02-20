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
        if (myColl != null)
        {
            speechBubble.SpeechBubbleOn(appearTitleKey, appearDialogSubKey, appearProbability);
            Init();
        }

    }

    public override void Init()
    {
        base.Init();

        arrowPos = transform.Find("ArrowPos");
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
