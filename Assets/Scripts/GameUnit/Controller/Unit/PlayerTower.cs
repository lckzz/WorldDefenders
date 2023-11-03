using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : Tower
{
    [SerializeField]
    private TowerState twState = TowerState.Idle;

    private int m_level = 1;

    TowerStat towerStat = new TowerStat();

    // Start is called before the first frame update
    void Start()
    {
        towerStat = Managers.Data.towerDict[GlobalData.g_PlayerLevel];
        base.Init(towerStat.hp ,towerStat.level);       //나중에 JSon으로 받은 타워의 데이터에서 hp와 level을 넣어줌
    }

    // Update is called once per frame
    void Update()
    {
        //if(Managers.Game.State != GameState.GameFail)
        //    Managers.Game.State = GameState.GameFail;

        if (Input.GetKeyDown(KeyCode.W))
            this.transform.DOLocalMoveX(-10.0f,0.07f).SetLoops(-1,LoopType.Yoyo);
        if (Input.GetKeyDown(KeyCode.E))
            DOTween.Kill(this.transform);
    }

    public float GetSetHp { get { return hp; } set { if (value > 0) hp = value; } }
    public float GetMaxHp { get { return maxHp; } }



    public override float hpPercent()
    {
        return hp / maxHp;
    }

    public override void TowerDamage(int att)
    {
        if (hp > 0)
        {
            hp -= att;

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
        if(twState == TowerState.Destroy)
        {
            Managers.Game.ResultState(Define.StageStageType.Defeat);

            //GameManager.instance.state = GameState.GameFail;
        }
    }



    public void TowerCameraShakeOn()
    {
        //타임라인 시그널 함수
        Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
        cameraCtrl?.Shake.VirtulCameraShake(1, 1,true);
    }

    public void TowerCameraShakeOff()
    {
        //타임라인 시그널 함수
        Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
        cameraCtrl?.Shake.VirtulCameraShake(0, 0, true);
    }

    public void TowerTrShakeOn()
    {
        //타임라인 시그널 함수
        this.transform.DOLocalMoveX(-10.0f, 0.07f).SetLoops(-1, LoopType.Yoyo);
    }

    public void TowerTrShakeOff()
    {
        //타임라인 시그널 함수

        DOTween.Kill(this.transform);
    }
}
