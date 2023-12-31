using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArrowCtrl : MonoBehaviour
{

    [SerializeField] private Unit unitCtrl;
    [SerializeField] private MonsterController monsterCtrl;
    private PlayerTower playerTower;
    private float arrowSpeed = 35.0f;

    [SerializeField] Vector3 shotDir;


    public void Init()
    {

        monsterCtrl = GetComponentInParent<MonsterController>();
        if(monsterCtrl.UnitCtrl != null)
            unitCtrl = monsterCtrl.UnitCtrl;
        if(monsterCtrl.PlayerTowerCtrl != null)
            playerTower = monsterCtrl.PlayerTowerCtrl;

    }


    private void OnDisable()
    {
        //꺼지면 다 초기화
        unitCtrl = null;
        monsterCtrl = null;
        playerTower = null;
        shotDir = Vector3.zero;
    }
    //private void OnEnable()
    //{
    //    if (monsterCtrl == null)
    //        return;


    //    if (monsterCtrl.UnitCtrl != null)
    //        unitCtrl = monsterCtrl.UnitCtrl;
    //    if (monsterCtrl.PlayerTowerCtrl != null)
    //        playerTower = monsterCtrl.PlayerTowerCtrl;
    //}

    // Start is called before the first frame update
    void Start()
    {
        Init();


    }

    // Update is called once per frame
    void Update()
    {
        if ((unitCtrl == null && playerTower == null) ||  monsterCtrl == null)
        {
            Destroy(this.gameObject);
            return;

        }


        Shot(unitCtrl, playerTower);

    }


    void Shot<T,T1>(T unit ,T1 tower) where T : Unit where T1 : PlayerTower
    {
        if (unit != null)
        {
            //Debug.Log("여기 유닛");
            shotDir = unit.transform.position - monsterCtrl.transform.position;
            shotDir.Normalize();

            if (unit != null || monsterCtrl != null)
                this.transform.position += shotDir * Time.deltaTime * arrowSpeed;


        }
        else if (tower != null)
        {
            //Debug.Log("여기 타워조준");

            shotDir = tower.transform.position - monsterCtrl.transform.position;
            shotDir.Normalize();

            if (tower != null || monsterCtrl != null)
                this.transform.position += shotDir * Time.deltaTime * arrowSpeed;
        }


        float angle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        angle += 180.0f;
        this.transform.eulerAngles = new Vector3(.0f, 0.0f, angle);
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (unitCtrl == null && playerTower == null)
            return;

        if (coll.tag == "Unit")
        {
            if (unitCtrl == null)
                return;

            Managers.Sound.Play("Sounds/Effect/Arrowhit");

            Unit uniCtrl = null;
            coll.TryGetComponent<Unit>(out uniCtrl);
            if (uniCtrl != null)
                uniCtrl.OnDamage(monsterCtrl.Att);

                
            


            StartCoroutine(Util.DestroyTime(gameObject));

        }
        else if(coll.tag.Contains("Tower") && coll.name.Contains("Tower"))
        {

            if (playerTower == null)
                return;

            Managers.Sound.Play("Sounds/Effect/Arrowhit");

            PlayerTower playTower = null;
            coll.TryGetComponent<PlayerTower>(out playTower);
            if (playTower != null)
                playTower.TowerDamage(monsterCtrl.Att);
                
            

            StartCoroutine(Util.DestroyTime(gameObject));

        }

        else
            return;
    }

}
