using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackCtrl : MonoBehaviour
{

    private SpecialUnitController unitCtrl;
    private Unit monsterCtrl;
    [SerializeField] private MonsterPortal monPortal;
    [SerializeField] private float magicSpeed = 20.0f;


    Vector3 shotDir;
    float Distance;
    Vector3 shotVec;


    void Init()
    {

        unitCtrl = GetComponentInParent<SpecialUnitController>();
        if (unitCtrl.Monctrl != null)
            monsterCtrl = unitCtrl.Monctrl;
        if (unitCtrl.MonsterPortal != null)
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
                this.transform.position += shotDir * Time.deltaTime * magicSpeed;


        }
        else if (tower != null)
        {

            shotDir = tower.transform.position - unitCtrl.transform.position;
            shotDir.Normalize();

            if (tower != null || unitCtrl != null)
                this.transform.position += shotDir * Time.deltaTime * magicSpeed;
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
                Unit monctrl = null;
                coll.TryGetComponent(out monctrl);
                if (monctrl != null && monsterCtrl == monctrl)
                    monctrl.OnDamage(unitCtrl.Att);

                Vector3 pos = coll.transform.position;
                pos.x -= 0.5f;
                MagicEffectAndSound(pos, "", "MagicHit");
            }


            Destroy(this.gameObject);
        }
        else if (coll.tag.Contains("Tower"))
        {

            if(coll.gameObject == monPortal.gameObject)
            {
                Tower monPort = null;
                coll.TryGetComponent<Tower>(out monPort);
                if (monPort != null)
                    monPort.TowerDamage(unitCtrl.Att);

                Vector3 pos = coll.transform.position;
                pos.x -= 0.5f;
                MagicEffectAndSound(pos, "", "MagicHit");


            }

            Destroy(this.gameObject);

        }

        else
            return;
    }



    //리소스매니저에 있어야할듯?
    void MagicEffectAndSound(Vector3 pos, string soundPath, string effPath)
    {
        //Managers.Sound.Play($"Sounds/Effect/{soundPath}");
        GameObject eff = Managers.Resource.Load<GameObject>($"Prefabs/Effect/{effPath}");
        

        if (eff != null)
            Instantiate(eff, pos, Quaternion.identity);
    }

    public void SetType(Unit monCtrl, MonsterPortal monPort)
    {
        monsterCtrl = monCtrl;
        monPortal = monPort;
    }

}
