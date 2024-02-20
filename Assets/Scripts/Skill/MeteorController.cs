using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : SkillBase
{

    Unit enemy;
    Vector3 pos;
    Vector3 curPos;
    float _speed = 20.0f;
    float _lifeTime = 3.0f;


    public void SetInfo(Unit owner,Unit enemy, SkillData data, Vector3 startPos)
    {
        //if (Managers.Data.magicSkillDict.TryGetValue(Unitlv, out SkillData data) == false)
        //{
        //    Debug.LogError("ProjecteController SetInfo Failed");
        //    return;
        //}

        Owner = owner;
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
        if (Owner == null || enemy.IsDie == true)
            Destroy(this.gameObject);

        curPos = Owner.transform.position;
        curPos.y += 2.0f;

        Vector3 shotDir = (enemy.transform.position - curPos).normalized;

        transform.position += shotDir * _speed * Time.deltaTime;

        float angle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        angle += 180.0f;
        this.transform.eulerAngles = new Vector3(.0f, 0.0f, angle);
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag.Contains("Monster"))
        {
            if(coll.gameObject == enemy.gameObject)
            {
                Managers.Sound.Play("Effect/Explosion");
                coll.TryGetComponent(out Unit mon);

                if(Owner.CriticalCheck())
                    mon.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)) * 2,0,true);      //넉백은 없고 크리티컬은 터짐
                else
                    mon.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)));      //노크리티컬

                Managers.Resource.ResourceEffect(coll.transform.position, "MeteorEff");
                Destroy(this.gameObject);
            }

        }
    }
}
