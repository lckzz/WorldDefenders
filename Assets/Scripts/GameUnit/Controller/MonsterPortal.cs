using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPortal : Tower
{
    [SerializeField]
    private TowerState twState = TowerState.Idle;

    [SerializeField]
    private float m_hp = 500;
    private int m_level = 1;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        base.Init(m_hp, m_level);       //나중에 JSon으로 받은 타워의 데이터에서 hp와 level을 넣어줌
        this.TryGetComponent(out anim);
        anim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("PortalDestroy"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
                Destroy(this.gameObject);
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
            if (m_hp <= 0)
            {
                m_hp = 0;
                twState = TowerState.Destroy;
                TowerDestroy();
            }
        }
    }

    protected override void TowerDestroy()
    {
        if (twState == TowerState.Destroy)
        {
            anim.SetTrigger("Destroy");
            Managers.Game.State = GameState.GameVictory;
        }
    }
}
