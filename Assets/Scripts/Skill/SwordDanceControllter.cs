using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDanceControllter : SkillBase
{
    Unit owener;
    List<Unit> enemy = new List<Unit>();
    Vector3 pos;
    Vector3 curPos;
    float _speed = 20.0f;
    float _lifeTime = 1.5f;
    int hitCount = 0;
    int hitMaxCount = 10;

    public SwordDanceControllter() : base(Define.SkillType.Count)
    {
    }

    public void SetInfo(int id, Unit owner, List<Unit> enemy, SkillData data, Vector3 startPos)
    {
        //if (Managers.Data.magicSkillDict.TryGetValue(Unitlv, out SkillData data) == false)
        //{
        //    Debug.LogError("ProjecteController SetInfo Failed");
        //    return;
        //}

        owener = owner;
        this.enemy = enemy;
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
        if (owener == null)
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
                unit.OnDamage(owener.Att * 2);
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
