using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    protected float hp = 0;
    protected float MaxHp = 0;
    protected int att = 0;

    protected float moveSpeed = .0f;
    protected bool isRun = false;
    protected bool isAtt = false;
    protected bool isDie = false;
    protected bool isTageting = false;

    protected float archerAttDis = .0f;
    protected Collider2D[] coll2d;
    protected Collider2D unitColl;



    [SerializeField]
    protected Transform pos;
    [SerializeField]
    protected Vector2 boxSize;

    



    

}
