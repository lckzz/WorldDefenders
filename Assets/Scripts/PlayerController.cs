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
    private enum PlayerControllerOrder
    {
        PlayerAim,
        AttackPos,
        PlayerSkillFever,
        PlayerDust
    }

    private Transform playerAimTr;
    private Transform attackPosTr;
    private PlayerState playerState = PlayerState.idle;
    private Animator anim;
    private SpriteRenderer spRend;
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
    private GameObject playerDust;


    //FireArrowSKill
    private GameObject playerSkillFeverObj;
    private bool skillOn = false;
    private float durationTime = 0.0f;
    private int fireArrowCount = 0;         //스킬사용시 카운트
    private Define.PlayerArrowType playerArrow = Define.PlayerArrowType.Normal;
    public Define.PlayerArrowType PlayerArrow { get { return playerArrow; } set { playerArrow = value; } }

    //WeaknessSkill
    private List<MonsterBase> enemys = new List<MonsterBase>();

    public int FireArrowCount { get { return fireArrowCount; } }
    public float DurationTime { get { return durationTime; } }
    public PlayerSkillData SkillData { get; set; }
    public SkillBook Skills { get; protected set; }

    //FireArrowSKill
    Tower playerTower;
    TowerStat tower = new TowerStat();

    private readonly int hashAnimAttack = Animator.StringToHash("Attack");
    private readonly int hashAnimIdle = Animator.StringToHash("Idle");
    private readonly int hashAnimShot = Animator.StringToHash("Shot");
    private readonly int hashAnimDie = Animator.StringToHash("Die");


    public int Att { get { return att; } }
    public float AttackTimer { get { return attackTimer; } }


    public PlayerState PlayerSt {
        get { return playerState; } 

    }

    // Start is called before the first frame update
    void Start()
    {
        tower = Managers.Data.towerDict[Managers.Game.PlayerLevel];
        att = tower.att;

        TryGetComponent<Animator>(out anim);
        TryGetComponent<LineRenderer>(out lr);
        TryGetComponent(out spRend);
        Skills = GetComponent<SkillBook>();
        playerAimTr = this.transform.GetChild((int)PlayerControllerOrder.PlayerAim).transform;
        attackPosTr = this.transform.GetChild((int)PlayerControllerOrder.AttackPos).transform;
        playerSkillFeverObj = this.transform.Find("PlayerSkillFever").gameObject;
        playerDust = transform.GetChild((int)PlayerControllerOrder.PlayerDust).gameObject;
        this.transform.parent.gameObject.transform.GetChild(0).TryGetComponent(out playerTower);

        Debug.Log(playerTower);

        startPlayerAimTr = playerAimTr.position;
        lr.startWidth = .1f;
        lr.endWidth = .1f;
        PlayerAimOnOff(false);


        if (Managers.Game.CurPlayerEquipSkill != Define.PlayerSkill.Count)   //스킬이 장착되어 있다면 스킬초기화
            SkillInit();   //스킬 초기화

        



    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.GameEndResult())       //게임이 끝났으면 리턴
            return;

        PlayerAimMove();
        AttackCoolTimer();
        AimLine();

    }

    void SkillInit()
    {
        SkillData = new PlayerSkillData();

        switch (Managers.Game.CurPlayerEquipSkill)
        {
            case Define.PlayerSkill.Heal:
                SkillData = Managers.Data.healSkillDict[(int)Managers.Game.TowerHealSkillLv];
                Skills.AddSkill<TowerHealSkill>();
                break;

            case Define.PlayerSkill.FireArrow:
                SkillData = Managers.Data.fireArrowSkillDict[(int)Managers.Game.FireArrowSkillLv];
                Skills.AddSkill<FireArrowSkill>();

                break;

            case Define.PlayerSkill.Weakness:
                SkillData = Managers.Data.weaknessSkillDict[(int)Managers.Game.WeaknessSkillLv];
                Skills.AddSkill<WeaknessSkill>();

                break;


        }
    }

    public void ActiveSkillUse()
    {
        //스킬을 누르면 장착된 스킬이 나가게 할것 (스킬북을 통해서)
        if (Skills.activeSkillList.Count > 0)
        {
            switch (Managers.Game.CurPlayerEquipSkill)
            {
                case Define.PlayerSkill.Heal:

                    Skills.activeSkillList[0].UseSkill(playerTower);     //스킬 사용

                    Managers.Sound.Play($"Effect/{SkillData.skillSound}");
                    break;

                case Define.PlayerSkill.FireArrow:

                    Skills.activeSkillList[0].UseSkill(this);     //스킬 사용
                    Managers.Sound.Play($"Effect/{SkillData.skillSound}");

                    break;

                case Define.PlayerSkill.Weakness:
                    Skills.activeSkillList[0].UseSkill(SkillMonsterExplore());     //스킬 사용
                    Managers.Sound.Play($"Effect/{SkillData.skillSound}");

                    break;
            }

        }

        SkillDuration();
    }


    public void AttackWait()           //버튼을 누르면 활쏠 준비를 한다.
    {
        if (Managers.Game.GameEndResult())       //게임이 끝났으면 리턴
            return;

        if (!attack)
            return;



        anim.SetBool(hashAnimIdle, false);
        anim.SetBool(hashAnimShot, false);
        anim.SetBool(hashAnimAttack, false);
        anim.Play(hashAnimAttack);



        ready1 = true;
        aimMove = true;
        lineSet = true;
        if (lineSet)                    //라인렌더러를 생성할지 판단
            lr.enabled = true;


        PlayerAimOnOff(true);           //플레이어의 에임을 켜준다.


    }


    public void ShotArrow()         //버튼에서 손을 땔때 공격(화살이 생성)
    {
        if (Managers.Game.GameEndResult())       //게임이 끝났으면 리턴
            return;


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

        ArrowInstance();                                            //화살을 생성

        playerAimTr.position = startPlayerAimTr;
        PlayerAimOnOff(false);                                      //에임을 꺼준다.

        ready1 = false;                                             //라인렌더러도 꺼준다.
        lineSet = false;
        if (!lineSet)
            lr.enabled = false;

        if (fireArrowCount > 0)                                             //폭발화살이 켜져있다면 폭발화살 카운트를 까준다.
            fireArrowCount--;

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


    public List<MonsterBase> SkillMonsterExplore()    //적 탐색
    {
        enemys.Clear();     //리스트안에 들어있는것들을 클리어해주고

        MonsterBase[] go = FindObjectsOfType<MonsterBase>();//GameObject.FindGameObjectsWithTag("Monster");     //현재 찾을수있는 몬스터태그를 가진 오브젝트들을 찾아서 배열에 넣어준다.

        for(int ii = 0; ii < go.Length; ii++)
        {
            enemys.Add(go[ii]);
        }

        return enemys;
    }

    public void PlayerDieStart()
    {
        anim.SetTrigger(hashAnimDie);
        spRend.flipX = true;
    }

    public void PlayerDieEnd()
    {
        playerDust.SetActive(true);
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
        if(Managers.Game.CurPlayerEquipSkill == Define.PlayerSkill.FireArrow)
        {

            fireArrowCount = 5;         //스킬사용되면 5개의 폭발화살 장착
            //skillOn = true;
            durationTime = SkillData.skillDuration;
            StartCoroutine(DurationSkillCoolTime());
            

        }
        else if(Managers.Game.CurPlayerEquipSkill == Define.PlayerSkill.Weakness)
        {
            durationTime = SkillData.skillDuration;
            
            StartCoroutine(DurationSkillCoolTime());
        }
    }


    IEnumerator DurationSkillCoolTime()
    {
        while(true)
        {
            durationTime -= Time.deltaTime;

            //skillOn = false;
            if (Managers.Game.CurPlayerEquipSkill == Define.PlayerSkill.FireArrow)
            {
                if (fireArrowCount <= 0)  //화살횟수를 다쓰면 스킬 종료
                {
                    playerArrow = Define.PlayerArrowType.Normal;
                    playerSkillFeverObj.SetActive(false);
                    fireArrowCount = 0;
                    yield break;

                }

            }


            if (durationTime <= 0.0f)  //지속시간이 지나면 스킬 종료
            {
                if(Managers.Game.CurPlayerEquipSkill == Define.PlayerSkill.FireArrow)   //폭발화살이라면 지속시간이 끝나면 기본화살로 바꾸기
                {
                    playerArrow = Define.PlayerArrowType.Normal;
                    playerSkillFeverObj.SetActive(false);
                    fireArrowCount = 0;
                }

                yield break;

            }



            yield return null;
        }
    }




}
