using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour,ISensor
{
    // Start is called before the first frame update
    [SerializeField]  protected float hp = 0;
    protected float maxHp = 0;
    [SerializeField]
    protected int att = 0;
    protected int knockbackForce = 0;
    protected int damageKnockBack = 0;
    protected float attackRange = .0f;

    protected float randomX = 0;
    protected float randomY = 0;

    protected float moveSpeed = .0f;
    protected bool isRun = false;
    protected bool isAtt = false;
    protected bool isDie = false;
    protected bool isIdle = false;


    [SerializeField]
    protected float attackCoolTime = 1.5f;
    protected bool attackTime = true;      //�������� �ٵ� ����

    [SerializeField]
    protected Collider2D[] coll2d;
    protected Collider2D tower;
    protected Collider2D unitColl;

    protected float unitDestroyTime = .0f;
    protected float distance;

    //Ÿ�� ���� ����
    protected Vector2 towerVec;
    protected Vector2 towerDir;
    protected float towerDist;
    protected bool towerTrace = false;
    protected bool towerAttack = false;       //Ÿ���� ��������� 

    //Ÿ�� ���� ����

    [SerializeField]
    protected Transform pos;
    [SerializeField]
    protected Vector2 boxSize;

    public virtual void EnemySensor()
    {
        //������ ����
    }

    public virtual void UnitDead()
    {
        if(unitDestroyTime > 0.0f)
        {
            unitDestroyTime -= Time.deltaTime;
            if(unitDestroyTime < 0.0f)
            {
                unitDestroyTime = .0f;

            }
        }
    }

    public virtual void AttackDelay() { }

    



    

}
