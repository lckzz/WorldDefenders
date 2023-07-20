using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using UnityEngine;

enum MonsterClass
{
    Warrior,
    Archer
}

enum MonsterState
{
    Idle,
    Run,
    Attack,
    Die
}

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterClass monsterClass;
    private MonsterState state = MonsterState.Run;        //테스트용이라 런 

    float moveSpeed = 1.0f;
    bool isRun = false;
    bool isAtt = false;
    float warriorRay = 1.0f;
    float archerRay = 1.0f;

    Rigidbody2D rig;
    RaycastHit2D hit;
    float archerAttDis = 0.5f;
    [SerializeField]
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(rig.position, Vector3.right, Color.red, 100.0f);

        if (monsterClass == MonsterClass.Warrior)
        {
            hit = Physics2D.Raycast(rig.position, Vector3.right, warriorRay, LayerMask.GetMask("Unit"));

        }
        else if (monsterClass == MonsterClass.Archer)
        {
            hit = Physics2D.Raycast(rig.position, Vector3.right, archerRay, LayerMask.GetMask("Unit"));

        }
        hit = Physics2D.Raycast(rig.position, Vector3.right, 10, LayerMask.GetMask("Unit"));

        switch (state)
        {
            case MonsterState.Run:
                if (!isRun)
                {
                    isRun = true;
                    anim.SetBool("Run", isRun);

                }
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                if (hit.collider != null)
                {
                    if (monsterClass == MonsterClass.Archer)
                    {
                        Debug.Log(hit.distance);

                        if (hit.distance < archerAttDis)
                        {
                            isRun = false;
                            anim.SetBool("Run", isRun);
                            state = MonsterState.Attack;
                        }
                    }
                    else if (monsterClass == MonsterClass.Warrior)
                    {
                        if (hit.distance < 0.5f)
                        {
                            isRun = false;
                            anim.SetBool("Run", isRun);
                            state = MonsterState.Attack;
                        }
                    }

                }
                break;
            case MonsterState.Attack:
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
    }
}
