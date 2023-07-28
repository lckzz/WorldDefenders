using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour,ISensor
{
    // Start is called before the first frame update
    protected float hp = 0;
    protected float maxHp = 0;
    [SerializeField]
    protected int att = 0;

    protected float moveSpeed = .0f;
    protected bool isRun = false;
    protected bool isAtt = false;
    protected bool isDie = false;
    protected bool isTargeting = false;         //��ü���� Ÿ������ ����
    protected bool isUnitTarget = false;        //����,���Ͱ� Ÿ���̴�.
    protected bool isTowerTarger = false;       //Ÿ���� Ÿ���̴�.

    [SerializeField]
    protected float attackCoolTime = 01.5f;
    protected bool attackTime = true;      //�������� �ٵ� ����

    protected float archerAttDis = .0f;
    protected Collider2D[] coll2d;
    protected Collider2D towerColl;
    protected Collider2D unitColl;

    protected float unitDestroyTime = .0f;


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
