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

    private int att = 15;               //���� �÷��̾� ���ݷ��� ��������
    private float attackTimer = .0f;
    private float attackMaxTimer = 0.5f;

    private bool attack = true;
    private Vector3 startPlayerAimTr;
    private float aimMaxDistance = 15.0f;       //�÷��̾� ������ �ִ��Ÿ�

    private Vector3 vecDir;
    private float dist;
    private Vector3 dir;

    private bool ready1 = false;            //���� ����������
    private bool ready2 = false;


    private LineRenderer lr;
    private bool lineSet = false;
    private GameObject playerDust;


    //FireArrowSKill
    private GameObject playerSkillFeverObj;
    private bool skillOn = false;
    private float durationTime = 0.0f;
    private int fireArrowCount = 0;         //��ų���� ī��Ʈ
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


        if (Managers.Game.CurPlayerEquipSkill != Define.PlayerSkill.Count)   //��ų�� �����Ǿ� �ִٸ� ��ų�ʱ�ȭ
            SkillInit();   //��ų �ʱ�ȭ

        



    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.GameEndResult())       //������ �������� ����
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
        //��ų�� ������ ������ ��ų�� ������ �Ұ� (��ų���� ���ؼ�)
        if (Skills.activeSkillList.Count > 0)
        {
            switch (Managers.Game.CurPlayerEquipSkill)
            {
                case Define.PlayerSkill.Heal:

                    Skills.activeSkillList[0].UseSkill(playerTower);     //��ų ���

                    Managers.Sound.Play($"Effect/{SkillData.skillSound}");
                    break;

                case Define.PlayerSkill.FireArrow:

                    Skills.activeSkillList[0].UseSkill(this);     //��ų ���
                    Managers.Sound.Play($"Effect/{SkillData.skillSound}");

                    break;

                case Define.PlayerSkill.Weakness:
                    Skills.activeSkillList[0].UseSkill(SkillMonsterExplore());     //��ų ���
                    Managers.Sound.Play($"Effect/{SkillData.skillSound}");

                    break;
            }

        }

        SkillDuration();
    }


    public void AttackWait()           //��ư�� ������ Ȱ�� �غ� �Ѵ�.
    {
        if (Managers.Game.GameEndResult())       //������ �������� ����
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
        if (lineSet)                    //���η������� �������� �Ǵ�
            lr.enabled = true;


        PlayerAimOnOff(true);           //�÷��̾��� ������ ���ش�.


    }


    public void ShotArrow()         //��ư���� ���� ���� ����(ȭ���� ����)
    {
        if (Managers.Game.GameEndResult())       //������ �������� ����
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

        ArrowInstance();                                            //ȭ���� ����

        playerAimTr.position = startPlayerAimTr;
        PlayerAimOnOff(false);                                      //������ ���ش�.

        ready1 = false;                                             //���η������� ���ش�.
        lineSet = false;
        if (!lineSet)
            lr.enabled = false;

        if (fireArrowCount > 0)                                             //����ȭ���� �����ִٸ� ����ȭ�� ī��Ʈ�� ���ش�.
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


    public List<MonsterBase> SkillMonsterExplore()    //�� Ž��
    {
        enemys.Clear();     //����Ʈ�ȿ� ����ִ°͵��� Ŭ�������ְ�

        MonsterBase[] go = FindObjectsOfType<MonsterBase>();//GameObject.FindGameObjectsWithTag("Monster");     //���� ã�����ִ� �����±׸� ���� ������Ʈ���� ã�Ƽ� �迭�� �־��ش�.

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

            fireArrowCount = 5;         //��ų���Ǹ� 5���� ����ȭ�� ����
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
                if (fireArrowCount <= 0)  //ȭ��Ƚ���� �پ��� ��ų ����
                {
                    playerArrow = Define.PlayerArrowType.Normal;
                    playerSkillFeverObj.SetActive(false);
                    fireArrowCount = 0;
                    yield break;

                }

            }


            if (durationTime <= 0.0f)  //���ӽð��� ������ ��ų ����
            {
                if(Managers.Game.CurPlayerEquipSkill == Define.PlayerSkill.FireArrow)   //����ȭ���̶�� ���ӽð��� ������ �⺻ȭ��� �ٲٱ�
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
