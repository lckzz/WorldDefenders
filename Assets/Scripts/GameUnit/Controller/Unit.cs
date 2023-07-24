using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour,ISensor
{
    // Start is called before the first frame update
    protected float hp = 0;
    protected float maxHp = 0;
    protected int att = 0;

    protected float moveSpeed = .0f;
    protected bool isRun = false;
    protected bool isAtt = false;
    protected bool isDie = false;
    protected bool isTargeting = false;

    protected float archerAttDis = .0f;
    protected Collider2D[] coll2d;
    protected Collider2D unitColl;

    protected float unitDestroyTime = .0f;


    [SerializeField]
    protected Transform pos;
    [SerializeField]
    protected Vector2 boxSize;

    public virtual void EnemySensor()
    {
        //적감지 센서
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

    



    

}
