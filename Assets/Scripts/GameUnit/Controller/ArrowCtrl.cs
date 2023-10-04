using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowCtrl : MonoBehaviour
{
    private Vector3 unitArrowScale = new Vector3(-5f, 5f, 5f);
    private UnitController unitCtrl;
    private Unit monsterCtrl;
    [SerializeField] private MonsterPortal monPortal;

    private float arrowSpeed = 35.0f;

    //화살은 쏘는 유닛의 정보 맞는 몬스터정보둘다 받아와야함
    Vector3 shotDir;
    float Distance;
    Vector3 shotVec;


    void Init()
    {
        transform.localScale = unitArrowScale;
       
        unitCtrl = GetComponentInParent<UnitController>();
        if(unitCtrl.Monctrl != null)
            monsterCtrl = unitCtrl.Monctrl;
        if(unitCtrl.MonsterPortal != null)
            monPortal = unitCtrl.MonsterPortal;
        

    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (unitCtrl == null || (monsterCtrl == null && monPortal == null))
        {
            Destroy(this.gameObject);
            return;

        }


        Shot(monsterCtrl, monPortal);

    }

    void Shot<T, T1>(T mon, T1 tower) where T : Unit where T1 : Tower
    {
        if (mon != null)
        {
            shotDir = mon.transform.position - unitCtrl.transform.position;
            shotDir.Normalize();

            if (mon != null || unitCtrl != null)
                this.transform.position += shotDir * Time.deltaTime * arrowSpeed;


        }
        else if (tower != null)
        {

            shotDir = tower.transform.position - unitCtrl.transform.position;
            shotDir.Normalize();

            if (tower != null || unitCtrl != null)
                this.transform.position += shotDir * Time.deltaTime * arrowSpeed;
        }


        float angle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        angle += 180.0f;
        this.transform.eulerAngles = new Vector3(.0f, 0.0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Monster")
        {

            if (coll.gameObject == monsterCtrl.gameObject)
            {
                Managers.Sound.Play("Sounds/Effect/Arrowhit");

                Unit monctrl = null;
                coll.TryGetComponent<Unit>(out monctrl);
                if (monctrl != null)
                    monctrl.OnDamage(unitCtrl.Att);
            }


            StartCoroutine(Util.UnitDieTime(gameObject));

        }
        else if (coll.tag.Contains("Tower") && coll.name.Contains("MonsterPortal"))
        {
            if (coll.gameObject == monPortal.gameObject)
            {
                Managers.Sound.Play("Sounds/Effect/Arrowhit");
                MonsterPortal monPort = null;
                coll.TryGetComponent<MonsterPortal>(out monPort);
                if (monPort != null)
                    monPort.TowerDamage(unitCtrl.Att);

                StartCoroutine(Util.UnitDieTime(gameObject));

            }
        }

        else
            return;
    }


    public void SetType(Unit monCtrl , MonsterPortal monPort)
    {
        monsterCtrl = monCtrl;
        monPortal = monPort;
    }

}
