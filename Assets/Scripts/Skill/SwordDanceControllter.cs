using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDanceControllter : SkillBase
{
    private List<Unit> enemy;
    private Vector3 pos;
    private Vector3 curPos;
    private float _speed = 20.0f;
    private float _lifeTime = 1.5f;
    private int hitCount = 0;
    private int hitMaxCount = 10;



    public void SetInfo(int id, Unit owner, List<Unit> enemy, SkillData data, Vector3 startPos)
    {
        //if (Managers.Data.magicSkillDict.TryGetValue(Unitlv, out SkillData data) == false)
        //{
        //    Debug.LogError("ProjecteController SetInfo Failed");
        //    return;
        //}

        Owner = owner;
        this.enemy = new List<Unit>(enemy);
        pos = startPos;
        SkillData = data;
        this.transform.position = pos;
        // TODO : Data Parsing
    }


    public override bool Init()
    {
        base.Init();

        StartDestroy(_lifeTime);

        return true;
    }

    public override void UpdateController()
    {
        base.UpdateController();

        //적이 사라지면 프리팹도 사라지게 해야댐
        if (Owner == null)
            Destroy(this.gameObject);



        transform.position +=  Vector3.left * _speed * Time.deltaTime;


    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Contains("Unit"))
        {

            if(hitCount < hitMaxCount)
            {
                coll.TryGetComponent(out Unit unit);

                if (Owner.CriticalCheck())
                    unit.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)) * 2, 0, true);      //넉백은 없고 크리티컬은 터짐
                else
                    unit.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)));      //노크리티컬

                Managers.Resource.ResourceEffect(coll.transform.position, "HitEff");
                hitCount++;
            }
            else if(hitCount > hitMaxCount)
            {
                GameObject.Destroy(this.gameObject, 0.5f);
            }






        }
    }
}
