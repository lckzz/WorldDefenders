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
        skillData = Managers.Data.fireArrowSkillDict[Managers.Game.FireArrowSkillLv];
        TryGetComponent(out coll2d);
        Camera.main.TryGetComponent(out CameraCtrl camCtrl);
        camCtrl.ShakeCamera(0.2f, 0.25f);

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Monster"))
        {
            //������ �Ͼ�� ���߹����� �ִٸ� �˹�� �������� �ش�.
            //coll.

            coll.TryGetComponent(out Unit enemy);
            if (enemy is MonsterController monCtrl)
            {
                if (monCtrl.Debuff is FireDebuff fireDebuff)
                {
                    monCtrl.OnDamage(skillData.skillValue, explosionKnockBack); //���ߵ������� �԰�
                    fireDebuff.DebuffOnOff(true, monCtrl);   //�� �Ŀ� �ʸ��� �������� �޴� ȭ�� ������Ʈ�� ���ش�.
                }


            }
            else if(enemy is EliteMonsterController eliteMonCtrl)
            {
                if (eliteMonCtrl.Debuff is FireDebuff fireDebuff)
                {
                    eliteMonCtrl.OnDamage(skillData.skillValue, explosionKnockBack); //���ߵ������� �԰�
                    fireDebuff.DebuffOnOff(true, eliteMonCtrl);   //�� �Ŀ� �ʸ��� �������� �޴� ȭ�� ������Ʈ�� ���ش�.
                }
          
            }

        }

    }




    //�ִϸ��̼� �̺�Ʈ �Լ�

    public void CollDisEnable()
    {
        coll2d.enabled = false;
    }


    public void ObjectDestroy()
    {
        Managers.Resource.Destroy(this.gameObject,1.0f);
    }

    //�ִϸ��̼� �̺�Ʈ �Լ�

}
