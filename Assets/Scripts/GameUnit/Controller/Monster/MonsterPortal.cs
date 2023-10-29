using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPortal : Tower
{
    [SerializeField]
    private TowerState twState = TowerState.Idle;

    private int m_level = 1;

    Animator anim;

    bool check = false;

    MonsterGateStat monsterGate = new MonsterGateStat();

    // Start is called before the first frame update
    void Start()
    {
        monsterGate = Managers.Data.monsterGateDict[(int)Managers.Game.CurStageType + 1];
        base.Init(monsterGate.hp, monsterGate.level);       //���߿� JSon���� ���� Ÿ���� �����Ϳ��� hp�� level�� �־���
        anim = this.GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if (anim.GetCurrentAnimatorStateInfo(0).IsName("PortalDestroy"))
    //    //{
    //    //    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
    //    //        Destroy(this.gameObject);
    //    //}

    //    //if(anim.GetCurrentAnimatorStateInfo(0).IsName("GenerateGate"))
    //    //{
    //    //    //����Ʈ�� ������ �ִϸ��̼��̶��
    //    //    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6f)
    //    //    {
    //    //        if(check == false)
    //    //        {
    //    //            check = true;
    //    //            Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
    //    //            cameraCtrl?.Shake.VirtulCameraShake(1,1);
    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        check = false;
    //    //        Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
    //    //        cameraCtrl?.Shake.VirtulCameraShake(0, 0);
    //    //    }
    //    //}

    //}


    public void OpenGateCameraShakeOn()
    {
        //Ÿ�Ӷ��� �ñ׳� �Լ�
        Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
        cameraCtrl?.Shake.VirtulCameraShake(1, 1);
    }

    public void OpenGateCameraShakeOff()
    {
        //Ÿ�Ӷ��� �ñ׳� �Լ�
        Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
        cameraCtrl?.Shake.VirtulCameraShake(0, 0);
    }


    public override float hpPercent()
    {

        return hp / maxHp;
    }

    public override void TowerDamage(int att)
    {
        
        if (hp > 0)
        {
            hp -= att;
            if(Managers.Game.FinalMonsterCheck() == false)          //���������̺갡 �������¶��
            {
                //����Ʈ���ͽ���Ÿ���� �ƴ϶��
                if(hpPercent() <= 0.3f)   //ü���� 30�۹̸��� �Ǹ�
                {
                    Managers.Game.SetMonSpawnType(Define.MonsterSpawnType.Final);  //���������̺���·� �ٲ��ش�.
                }
            }

            if (hp <= 0)
            {
                twState = TowerState.Destroy;
                TowerDestroy();
                hp = 0;

            }
        }
    }

    protected override void TowerDestroy()
    {
        if (twState == TowerState.Destroy)
        {   

            //����Ʈ�� �ı����¶�� �¸� �ִϸ��̼ǰ� ����˾��� Ų��.
            anim.SetTrigger("Destroy");
            Managers.Game.ResultState(Define.StageStageType.Victory);   
        }
    }
}
