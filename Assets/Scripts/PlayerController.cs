using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    idle,
    Attack
}


public class PlayerController : MonoBehaviour
{
    private Transform playerAimTr;
    private Transform attackPosTr;
    private PlayerState playerState = PlayerState.idle;
    private Animator anim;
    private bool attackCheck = false;
    private bool aimMove = false;
    private float speed = 10.0f;

    private int att = 15;               //아직 플레이어 공격력은 아직아직
    private float attackTimer = .0f;
    private float attackMaxTimer = 1.5f;

    private bool attack = true;
    private Vector3 startPlayerAimTr;


    private Vector3 vecDir;
    private float dist;
    private Vector3 dir;

    private bool ready1 = false;            //공격 순서때문에
    private bool ready2 = false;


    private LineRenderer lr;
    private bool lineSet = false;


    public int Att { get { return att; } }
    public float AttackTimer { get { return attackTimer; } }


    public PlayerState PlayerSt {
        get { return playerState; } 

    }

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Animator>(out anim);
        TryGetComponent<LineRenderer>(out lr);
        playerAimTr = this.transform.GetChild(0).transform;
        attackPosTr = this.transform.GetChild(1).transform;

        startPlayerAimTr = playerAimTr.position;
        lr.startWidth = .1f;
        lr.endWidth = .1f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIdle();
        PlayerAimMove();
        AttackCoolTimer();
        AimLine();
    }




    public void AttackWait()           //버튼을 누르면 활쏠 준비를 한다.
    {
        if (!attack)
            return;


        PlayerAimOnOff(true);
        anim.SetBool("Idle", false);
        anim.SetBool("Shot", false);
        anim.SetBool("Attack", true);

        ready1 = true;
        aimMove = true;
        lineSet = true;
        if (lineSet)
            lr.enabled = true;


    }


    public void ShotArrow()         //버튼에서 손을 땔때 공격(화살이 생성)
    {
        if (!attack || !ready1)
            return;

        aimMove = false;
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Shot", true);

        vecDir = playerAimTr.position - attackPosTr.position;
        dist = vecDir.magnitude;
        dir = vecDir.normalized;

        ArrowInstance();

        ready1 = false;
        lineSet = false;
        if (!lineSet)
            lr.enabled = false;

    }

    void PlayerIdle()
    {

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("AimShoot"))
        {
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                attackCheck = true;
                playerAimTr.position = startPlayerAimTr;
            }
;        }


        if (attackCheck)
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Shot", false);
            anim.SetBool("Idle",true);
            attackCheck = false;
            PlayerAimOnOff(false);

        }

    }


    void PlayerAimMove()
    {
        if (!attack)
            return;


        if (aimMove)
            playerAimTr.transform.position += Vector3.right * Time.deltaTime * speed;
    }


    void ArrowInstance()
    {
        attack = false;
        attackTimer = 1.5f;

        GameObject go = Resources.Load<GameObject>("Prefab/Weapon/PlayerArrow");
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += 180.0f;
        go.transform.eulerAngles = new Vector3(.0f, 0.0f, angle);
        Instantiate(go, attackPosTr.position,go.transform.rotation);
    }


    void AttackCoolTimer()
    {
        if(attackTimer > .0f)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer < .0f)
            {
                attack = true;
                
            }
        }
    }

    void AimLine()
    {
        if(lineSet)
        {
            lr.SetPosition(0, attackPosTr.position);
            lr.SetPosition(1, playerAimTr.position);
        }
    }


    void PlayerAimOnOff(bool sw)
    {
        GameObject go = playerAimTr.gameObject;
        if (sw) //스위치가 켜지면 플레이어 에임이 켜짐
        {
            if (!go.activeSelf)
                go.SetActive(sw);
        }
        else
        {
            if (go.activeSelf)
                go.SetActive(sw);
        }
    }

    public void ArrowInfo(ref Vector3 Vecdir)
    {
       
        Vecdir = dir;
    }

    public float PercentCoolTime()
    {
        return attackTimer / attackMaxTimer;
    }
}
