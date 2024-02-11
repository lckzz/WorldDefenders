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
        towerStat = Managers.Data.towerDict[Managers.Game.PlayerLevel];
        base.Init(towerStat.hp ,towerStat.level);       //���߿� JSon���� ���� Ÿ���� �����Ϳ��� hp�� level�� �־���
    }

    // Update is called once per frame
    void Update()
    {
        //if(Managers.Game.State != GameState.GameFail)
        //    Managers.Game.State = GameState.GameFail;
    }

    public float Hp { get { return hp; } set { if (value > 0) hp = value; } }
    public float MaxHp { get { return maxHp; } }



    //public override float hpPercent()
    //{
    //    return hp / maxHp;
    //}

    public override void TowerDamage(int att)
    {
        if (hp > 0)
        {
            hp -= att;
            NotifyToHpObserver();

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
            if (Managers.Scene.CurrentScene as GameScene)
            {
                GameScene gameScene = (GameScene)Managers.Scene.CurrentScene;
                gameScene.GameDirector(Define.GameStageDirector.Defeat);
                Managers.Game.SetStageStateType(Define.StageStageType.Defeat);
            }
        }
    }



    public void TowerCameraShakeOn()
    {
        //Ÿ�Ӷ��� �ñ׳� �Լ�
        Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
        cameraCtrl?.Shake.VirtulCameraShake(1, 1,true);
    }

    public void TowerCameraShakeOff()
    {
        //Ÿ�Ӷ��� �ñ׳� �Լ�
        Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
        cameraCtrl?.Shake.VirtulCameraShake(0, 0, true);
    }

    public void TowerTrShakeOn()
    {
        //Ÿ�Ӷ��� �ñ׳� �Լ�
        this.transform.DOLocalMoveX(-10.0f, 0.07f).SetLoops(-1, LoopType.Yoyo);
    }

    public void TowerTrShakeOff()
    {
        //Ÿ�Ӷ��� �ñ׳� �Լ�

        DOTween.Kill(this.transform);
    }
}
