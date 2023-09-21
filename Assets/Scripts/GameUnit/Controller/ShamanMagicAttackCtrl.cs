using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanMagicAttackCtrl : MonoBehaviour
{
    private EliteMonsterController monCtrl;
    private Unit unitCtrl;
    [SerializeField] private PlayerTower tower;
    [SerializeField] private float magicSpeed = 20.0f;


    Vector3 shotDir;
    float Distance;
    Vector3 shotVec;


    void Init()
    {

        monCtrl = GetComponentInParent<EliteMonsterController>();
        if (monCtrl.UnitCtrl != null)
            unitCtrl = monCtrl.UnitCtrl;
        if (monCtrl.PlayerTowerCtrl != null)
            tower = monCtrl.PlayerTowerCtrl;


    }
    // Start is called before the first frame update
    void Start()
    {
        Init();

    }

    // Update is called once per frame
    void Update()
    {
        if (monCtrl == null || (unitCtrl == null && tower == null))
        {
            Destroy(this.gameObject);
            return;

        }


        Shot(unitCtrl, tower);
    }

    void Shot<T, T1>(T mon, T1 tower) where T : Unit where T1 : Tower
    {
        if (mon != null)
        {
            
            shotDir = mon.transform.position - monCtrl.transform.position;
            shotDir.Normalize();

            if (mon != null || monCtrl != null)
                this.transform.position += shotDir * Time.deltaTime * magicSpeed;


        }
        else if (tower != null)
        {
            Debug.Log("qwewqe");
            shotDir = tower.transform.position - monCtrl.transform.position;
            shotDir.Normalize();

            if (tower != null || monCtrl != null)
                this.transform.position += shotDir * Time.deltaTime * magicSpeed;
        }


    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Unit")
        {

            if (unitCtrl != null)
            {
                if (coll.gameObject == unitCtrl.gameObject)
                {
                    Unit ctrl = null;
                    coll.TryGetComponent(out ctrl);
                    if (ctrl != null && unitCtrl == ctrl)
                        ctrl.OnDamage(monCtrl.Att);

                    Vector3 pos = coll.transform.position;
                    pos.x -= 0.5f;
                    MagicEffectAndSound(pos, "", "ShamanMagicHit");
                }
            }



            Destroy(this.gameObject);
        }
        else if (coll.tag.Contains("Tower") && coll.name.Contains("Tower"))
        {
            Debug.Log("wqeqe");
            
            if (tower != null)
            {

                Debug.Log(LayerMask.GetMask("MonsterPortal"));
                if (coll.gameObject == tower.gameObject)
                {
                    Tower playerTower = null;
                    coll.TryGetComponent<Tower>(out playerTower);
                    if (playerTower != null)
                        playerTower.TowerDamage(monCtrl.Att);

                    Vector3 pos = coll.transform.position;
                    pos.x -= 0.5f;
                    MagicEffectAndSound(pos, "", "ShamanMagicHit");


                }
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

    public void SetType(Unit unit, PlayerTower playertower)
    {
        unitCtrl = unit;
        tower = playertower;
    }
}
