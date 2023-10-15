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
        base.Init(m_hp, m_level);       //���߿� JSon���� ���� Ÿ���� �����Ϳ��� hp�� level�� �־���
        anim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("PortalDestroy"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
                Destroy(this.gameObject);
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("GenerateGate"))
        {
            //����Ʈ�� ������ �ִϸ��̼��̶��
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6f)
            {
                if(check == false)
                {
                    check = true;
                    Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
                    cameraCtrl?.Shake.VirtulCameraShake(1,1);
                }
            }
            else
            {
                check = false;
                Camera.main.TryGetComponent(out CameraCtrl cameraCtrl);
                cameraCtrl?.Shake.VirtulCameraShake(0, 0);
            }
        }

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
            if(Managers.Game.EliteMonsterCheck() == false)
            {
                //����Ʈ���ͽ���Ÿ���� �ƴ϶��
                if(hpPercent() <= 0.3f)   //ü���� 30�۹̸��� �Ǹ�
                {
                    Debug.Log("erwewr");
                    Managers.Game.EliteMonsterEvent();
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
            anim.SetTrigger("Destroy");
            //GameManager.instance.State = GameState.GameVictory;
        }
    }
}
