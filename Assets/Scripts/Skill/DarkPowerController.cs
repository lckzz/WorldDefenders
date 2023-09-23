using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPowerController : SkillBase
{
    //최대 3명의 적을 받아와서 해당 스킬의 위치로 끌어당긴뒤 3번공격하고 스턴 상태이상을 준다.


    private Unit owener;
    [SerializeField] private List<Unit> enemy;





    public DarkPowerController() : base(Define.SkillType.Count)
    {

    }

    public void SetInfo(Unit owner, List<Unit> enemy, SkillData data)
    {
        //if (Managers.Data.magicSkillDict.TryGetValue(Unitlv, out SkillData data) == false)
        //{
        //    Debug.LogError("ProjecteController SetInfo Failed");
        //    return;
        //}

        owener = owner;
        this.enemy = enemy;
        SkillData = data;
        if (enemy.Count > 0)
            this.transform.position = enemy[0].transform.position;
        // TODO : Data Parsing
    }



    public override void UpdateController()
    {
        base.UpdateController();

        //적이 사라지면 프리팹도 사라지게 해야댐
        if (owener == null)
            Destroy(this.gameObject);


        if(enemy.Count > 0)
        {
            //몬스터가 있다면
            for(int ii = 0; ii < enemy.Count; ii++)
            {
                if (!enemy[ii].IsDie)
                {
                    enemy[ii].transform.position = Vector3.Lerp(enemy[ii].transform.position, this.transform.position, Time.deltaTime * 2.0f);
                }
            }
        }




    }

    public void GameObjectDestroy()
    {
        Destroy(this.gameObject);
    }


    public void SkillAttack()
    {
        for (int ii = 0; ii < enemy.Count; ii++)
        {
            enemy[ii].gameObject.TryGetComponent(out Unit mon);
            mon.OnDamage(10);
            Managers.Resource.ResourceEffect(enemy[ii].gameObject.transform.position, "HitEff");
            
        }
    }

}
