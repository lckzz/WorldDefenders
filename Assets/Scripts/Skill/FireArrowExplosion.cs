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
            //������ �Ͼ�� ���߹����� �ִٸ� �˹�� �������� �ش�.
            //coll.

            coll.TryGetComponent(out Unit enemy);
            if (enemy is MonsterController monCtrl)
            {
                monCtrl.OnDamage(skillData.skillValue, explosionKnockBack); //���ߵ������� �԰�
                monCtrl.Debuff.FireDebuffOnOff(true,monCtrl);   //�� �Ŀ� �ʸ��� �������� �޴� ȭ�� ������Ʈ�� ���ش�.

            }
            else if(enemy is EliteMonsterController eliteMonCtrl)
            {
                eliteMonCtrl.OnDamage(skillData.skillValue, explosionKnockBack); //���ߵ������� �԰�
                eliteMonCtrl.Debuff.FireDebuffOnOff(true,eliteMonCtrl);   //�� �Ŀ� �ʸ��� �������� �޴� ȭ�� ������Ʈ�� ���ش�.
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
