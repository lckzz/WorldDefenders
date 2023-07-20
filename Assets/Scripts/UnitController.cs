using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum UnitClass
{
    Warrior,
    Archer
}


enum UnitState
{
    Idle,
    Run,
    Attack,
    Die
}

public class UnitController : MonoBehaviour
{

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private UnitClass unitClass;

    private UnitState state = UnitState.Run;        //테스트용이라 런 

    float moveSpeed = 1.0f;
    bool isRun = false;
    bool isAtt = false;

    float archerAttDis = 1.5f;
    float warriorRay = 1.0f;
    float archerRay = 3.0f;

    Rigidbody2D rig;
    
    RaycastHit2D hit;
    AnimatorStateInfo animState;

    // Start is called before the first frame update
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        anim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(rig.position, Vector3.right, Color.red);
        if(unitClass == UnitClass.Warrior)
        {
            hit = Physics2D.Raycast(rig.position, Vector3.right, warriorRay, LayerMask.GetMask("Monster"));

        }
        else if(unitClass == UnitClass.Archer)
        {
            hit = Physics2D.Raycast(rig.position, Vector3.right, archerRay, LayerMask.GetMask("Monster"));

        }

        switch (state)
        {
            case UnitState.Run:
                if(!isRun)
                {
                    isRun = true;
                    anim.SetBool("Run", isRun);

                }
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;

                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.gameObject.name);

                    if (unitClass == UnitClass.Archer)
                    {

                        if (hit.distance < archerAttDis)
                        {
                            isRun = false;
                            anim.SetBool("Run", isRun);
                            state = UnitState.Attack;
                        }
                    }
                    else if(unitClass == UnitClass.Warrior)
                    {
                        if (hit.distance < 0.5f)
                        {
                            isRun = false;
                            anim.SetBool("Run", isRun);
                            state = UnitState.Attack;
                        }
                    }
   
                }
                break;
            case UnitState.Attack:
                if (!isAtt)
                {
                    isAtt = true;
                    anim.SetBool("Attack", isAtt);

                }
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    isAtt = false;
                    anim.SetBool("Attack", isAtt);
                }

                break;


        }

        //앞으로 움직임 
    }
}
