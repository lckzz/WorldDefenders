using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    private Vector3 unitArrowScale = new Vector3(-5f, 5f, 5f);
    private UnitController unitCtrl;
    private MonsterController monsterCtrl;
    private float arrowSpeed = 35.0f;

    //화살은 쏘는 유닛의 정보 맞는 몬스터정보둘다 받아와야함
    Vector3 shotDir;
    float Distance;
    Vector3 shotVec;


    void Init()
    {
        transform.localScale = unitArrowScale;
       
        unitCtrl = GetComponentInParent<UnitController>();
        monsterCtrl = unitCtrl.Monctrl;
        

    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (unitCtrl == null || monsterCtrl == null)
        {
            Destroy(this.gameObject);
            return;

        }



        shotDir = monsterCtrl.transform.position - unitCtrl.transform.position;
        shotDir.Normalize();

        if (unitCtrl != null || monsterCtrl != null)
            this.transform.position += shotDir * Time.deltaTime * arrowSpeed;
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Monster")
        {
            MonsterController monctrl = null;
            coll.TryGetComponent<MonsterController>(out monctrl);
            if (monctrl != null)
                monctrl.OnDamage(unitCtrl.Att);

            

            Destroy(this.gameObject);
        }
        else
            return;
    }

}
