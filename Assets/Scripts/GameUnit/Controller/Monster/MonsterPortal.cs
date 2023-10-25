using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPortal : Tower
{
    [SerializeField]
    private TowerState twState = TowerState.Idle;

    private float m_hp = 500;

    private int m_level = 1;

    Animator anim;

    bool check = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Init(m_hp, m_level);       //나중에 JSon으로 받은 타워의 데이터에서 hp와 level을 넣어줌
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
    //    //    //게이트가 열리는 애니메이션이라면
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
        //타임라인 시그널 함수
        Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
        cameraCtrl?.Shake.VirtulCameraShake(1, 1);
    }

    public void OpenGateCameraShakeOff()
    {
        //타임라인 시그널 함수
        Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
        cameraCtrl?.Shake.VirtulCameraShake(0, 0);
    }


    public override float hpPercent()
    {
        return m_hp / maxHp;
    }

    public override void TowerDamage(int att)
    {
        
        if (m_hp > 0)
        {
            m_hp -= att;
            if(Managers.Game.FinalMonsterCheck() == false)          //마지막웨이브가 꺼진상태라면
            {
                //엘리트몬스터스폰타입이 아니라면
                if(hpPercent() <= 0.3f)   //체력이 30퍼미만이 되면
                {
                    Managers.Game.SetMonSpawnType(Define.MonsterSpawnType.Final);  //마지막웨이브상태로 바꿔준다.
                }
            }

            if (m_hp <= 0)
            {
                twState = TowerState.Destroy;
                TowerDestroy();
                m_hp = 0;

            }
        }
    }

    protected override void TowerDestroy()
    {
        if (twState == TowerState.Destroy)
        {   

            //게이트가 파괴상태라면 승리 애니메이션과 결과팝업을 킨다.
            anim.SetTrigger("Destroy");
            Managers.Game.ResultState(Define.StageStageType.Victory);   
        }
    }
}
