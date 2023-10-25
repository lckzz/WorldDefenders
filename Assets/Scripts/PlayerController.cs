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
    private float attackMaxTimer = 0.5f;

    private bool attack = true;
    private Vector3 startPlayerAimTr;
    private float aimMaxDistance = 15.0f;       //플레이어 조준의 최대사거리

    private Vector3 vecDir;
    private float dist;
    private Vector3 dir;

    private bool ready1 = false;            //공격 순서때문에
    private bool ready2 = false;


    private LineRenderer lr;
    private bool lineSet = false;


    //FireArrowSKill
    private GameObject playerSkillFeverObj;
    private bool skillOn = false;
    private float durationTime = 0.0f;
    private Define.PlayerArrowType playerArrow = Define.PlayerArrowType.Normal;
    public Define.PlayerArrowType PlayerArrow { get { return playerArrow; } set { playerArrow = value; } }

    public float DurationTime { get { return durationTime; } }
    public SkillData SkillData { get; set; }
    //FireArrowSKill


    private readonly int hashAnimAttack = Animator.StringToHash("Attack");
    private readonly int hashAnimIdle = Animator.StringToHash("Idle");
    private readonly int hashAnimShot = Animator.StringToHash("Shot");


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
        playerSkillFeverObj = this.transform.Find("PlayerSkillFever").gameObject;

        startPlayerAimTr = playerAimTr.position;
        lr.startWidth = .1f;
        lr.endWidth = .1f;
        PlayerAimOnOff(false);

    }

    // Update is called once per frame
    void Update()
    {
        PlayerAimMove();
        AttackCoolTimer();
        AimLine();
        SkillDuration();
    }




    public void AttackWait()           //버튼을 누르면 활쏠 준비를 한다.
    {
        if (!attack)
            return;



        anim.SetBool(hashAnimIdle, false);
        anim.SetBool(hashAnimShot, false);
        anim.SetBool(hashAnimAttack, false);
        anim.Play(hashAnimAttack);



        ready1 = true;
        aimMove = true;
        lineSet = true;
        if (lineSet)
            lr.enabled = true;


        PlayerAimOnOff(true);


    }


    public void ShotArrow()         //버튼에서 손을 땔때 공격(화살이 생성)
    {
        if (!attack || !ready1)
            return;


        Managers.Resource.ResourceSound("Bow");
        aimMove = false;
        anim.SetBool(hashAnimAttack, false);
        anim.SetBool(hashAnimIdle, false);
        anim.SetBool(hashAnimShot, true);

        vecDir = playerAimTr.position - attackPosTr.position;
        dist = vecDir.magnitude;
        dir = vecDir.normalized;

        ArrowInstance();

        playerAimTr.position = startPlayerAimTr;
        PlayerAimOnOff(false);

        ready1 = false;
        lineSet = false;
        if (!lineSet)
            lr.enabled = false;

    }

    public void ArrowInfo(ref Vector3 Vecdir)
    {

        Vecdir = dir;
    }

    public float PercentCoolTime()
    {
        return attackTimer / attackMaxTimer;
    }

    public void SkillOnOffPlayerFever(bool check)
    {
        if (playerSkillFeverObj != null)
            playerSkillFeverObj.SetActive(check);
    }


    void PlayerAimMove()
    {
        if (!attack)
            return;



        if (aimMove)
        {
            float distance = (playerAimTr.position - attackPosTr.position).magnitude;
            if (distance < 15.0f)
                playerAimTr.position += Vector3.right * Time.deltaTime * speed;
            else
                playerAimTr.position = playerAimTr.position;
        }
    }


    void ArrowInstance()
    {
        attack = false;
        attackTimer = 0.5f;
        GameObject go;
        if (playerArrow == Define.PlayerArrowType.Normal)
        {
            go = Managers.Resource.Load<GameObject>("Prefabs/Weapon/PlayerArrow");
        }

        else
        {
            go = Managers.Resource.Load<GameObject>("Prefabs/Weapon/FireArrow");

        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += 180.0f;
        go.transform.eulerAngles = new Vector3(.0f, 0.0f, angle);
        Managers.Resource.Instantiate(go, attackPosTr.position, go.transform.rotation);


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

        go.SetActive(sw);
        
    }

    void SkillDuration()
    {
        if(playerSkillFeverObj.activeSelf == true)
        {
            //이 오브젝트가 켜지면 스킬이 써졌다는것
            if(skillOn == false)
            {
                skillOn = true;
                durationTime = SkillData.skillValue;
                StartCoroutine(DurationSkillTime());
            }

        }
    }


    IEnumerator DurationSkillTime()
    {
        while(true)
        {
            durationTime -= Time.deltaTime;


            if(durationTime <= 0.0f)
            {
                skillOn = false;
                playerSkillFeverObj.SetActive(false);
                playerArrow = Define.PlayerArrowType.Normal;
                yield break;
            }


            yield return null;
        }
    }


}
