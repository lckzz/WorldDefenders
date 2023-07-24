using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArrowCtrl : MonoBehaviour
{

    private UnitController unitCtrl;
    private MonsterController monsterCtrl;
    private float arrowSpeed = 35.0f;

    Vector3 shotDir;


    void Init()
    {
        monsterCtrl = GetComponentInParent<MonsterController>();
        unitCtrl = monsterCtrl.UnitCtrl;

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



        shotDir = unitCtrl.transform.position - monsterCtrl.transform.position;
        shotDir.Normalize();

        if (unitCtrl != null || monsterCtrl != null)
            this.transform.position += shotDir * Time.deltaTime * arrowSpeed;

        float angle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        angle += 180.0f;
        this.transform.eulerAngles = new Vector3(.0f, 0.0f, angle);
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Unit")
        {
            UnitController unitCtrl = null;
            coll.TryGetComponent<UnitController>(out unitCtrl);
            if (unitCtrl != null)
                unitCtrl.OnDamage(monsterCtrl.Att);



            Destroy(this.gameObject);
        }
        else
            return;
    }
}
