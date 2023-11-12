using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrowExplosion : MonoBehaviour
{
    private PlayerController player;
    private SkillData skillData;
    private int explosionKnockBack = 50;
    private Collider2D coll2d;

    private void Start()
    {
        GameObject.Find("Leader").transform.GetChild(1).TryGetComponent(out player);
        skillData = new SkillData();
        skillData = Managers.Data.fireArrowSkillDict[GlobalData.g_SkillFireArrowLv];
        TryGetComponent(out coll2d);
        Camera.main.TryGetComponent(out CameraCtrl camCtrl);
        camCtrl.ShakeCamera(0.2f, 0.25f);

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Monster"))
        {
            //폭발이 일어날때 폭발범위에 있다면 넉백과 데미지를 준다.
            //coll.

            coll.TryGetComponent(out Unit enemy);
            if (enemy is MonsterController monCtrl)
            {
                monCtrl.OnDamage(skillData.skillValue, explosionKnockBack); //폭발데미지를 입고
                monCtrl.Debuff.FireDebuffOnOff(true,monCtrl);   //그 후엔 초마다 데미지를 받는 화상 오브젝트를 켜준다.

            }
            else if(enemy is EliteMonsterController eliteMonCtrl)
            {
                eliteMonCtrl.OnDamage(skillData.skillValue, explosionKnockBack); //폭발데미지를 입고
                eliteMonCtrl.Debuff.FireDebuffOnOff(true,eliteMonCtrl);   //그 후엔 초마다 데미지를 받는 화상 오브젝트를 켜준다.
            }

        }

    }




    //애니메이션 이벤트 함수

    public void CollDisEnable()
    {
        coll2d.enabled = false;
    }


    public void ObjectDestroy()
    {
        Managers.Resource.Destroy(this.gameObject,1.0f);
    }

    //애니메이션 이벤트 함수

}
