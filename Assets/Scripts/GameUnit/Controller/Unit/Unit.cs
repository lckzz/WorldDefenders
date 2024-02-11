using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public abstract class Unit : MonoBehaviour,ISensor, IHpSubject
{
    // Start is called before the first frame update
    [SerializeField]  protected float hp = 0;
    protected float maxHp = 0;
    [SerializeField]
    protected int att = 0;
    protected int knockbackForce = 0;
    protected int damageKnockBack = 0;
    protected float attackRange = .0f;
    protected float spawnPosX = 0;       //��ȯ����x�� ���Ϸδ� �˹����������



    protected float randomX = 0;
    protected float randomY = 0;

    protected float moveSpeed = .0f;
    protected bool isRun = false;
    protected bool isAtt = false;
    [SerializeField]
    protected bool isDie = false;
    protected bool isIdle = false;

    [SerializeField]
    protected Animator anim; 
    protected Rigidbody2D rigbody;
    protected SpriteRenderer sp;

    [SerializeField]
    protected float attackCoolTime = 1.5f;
    protected bool attackTime = true;      //�������� �ٵ� ����

 
    protected Collider2D[] enemyColls2D;
    protected Collider2D towerColl;
    protected Collider2D unitColl;
    protected Collider2D myColl;     //�� �ڽ��� �ݶ��̴��� �޴� ����

    protected float unitDestroyTime = .0f;
    protected float traceDistance;


    [SerializeField]
    protected Transform pos;
    [SerializeField]
    protected Vector2 boxSize;

    protected UnitHp unitHUDHp;

    //Hp�� ��ü�ν� ���������� 
    private List<IHpObserver> observerList = new List<IHpObserver>();



    //�˹� ���� ����
    protected bool knockbackStart = false;
    protected float knockbackDuration = 0.25f;
    protected bool isNoKnockBack = false;           //�˹��� �ȵǴ� ������ �ִٸ�


    //��ǳ��
    protected SpeechBubble speechBubble;

    //HUDUI
    private Transform parentTr;
    private GameObject hudPrefab;

    //������ spriteRender������ ���̵�ƿ�
    protected UnityEngine.Color color = new UnityEngine.Color(0, 0, 0);

    protected float destoryTimer = 1.5f;
    protected bool startImgFadeOut = false;


    public bool IsDie { get { return isDie; } }
    public int Att { get { return att; } }
    public float Hp { get { return hp; } }
    public float MaxHp { get { return maxHp; } }
    public float MoveSpeed { get { return moveSpeed; } }

    public bool IsNoKnockBack { get { return isNoKnockBack; } set { isNoKnockBack = value; } }

    public Action onDead;

    public virtual void Init()
    {
        TryGetComponent<Animator>(out anim);
        TryGetComponent<Rigidbody2D>(out rigbody);
        TryGetComponent<Collider2D>(out myColl);
        TryGetComponent<SpriteRenderer>(out sp);
        TryGetComponent(out speechBubble);

        TryGetComponent<UnitHp>(out unitHUDHp);
        if (unitHUDHp == null)
            this.gameObject.AddComponent<UnitHp>().TryGetComponent(out unitHUDHp);


        parentTr = GameObject.Find("HUD_Canvas").transform;
        hudPrefab = Managers.Resource.Load<GameObject>("Prefabs/HUDDamage/DamageTxt");
        unitHUDHp.Init(parentTr, hudPrefab);

        Vector3 vec = this.gameObject.transform.position;
        vec.z = 0.0f;
        gameObject.transform.position = vec;        //Ȥ�ö� ������ z���� 0�� �ƴҶ��� ����ؼ� 0���� �־���
    }

    public float hpPercent()
    {
        return hp / maxHp;
    }


    public bool NoKnockBackValid()
    {
        if(isNoKnockBack == false)
            return false;

        return true;
    }

    protected IEnumerator UnitDeadSrAlpha()
    {
        while (true)
        {

            if (destoryTimer > 0.0f)
            {
                destoryTimer -= Time.deltaTime;
                if (destoryTimer < .0f)
                {
                    destoryTimer = .0f;
                    startImgFadeOut = true;

                }
            }


            if (startImgFadeOut)
            {

                color = sp.color;
                if (color.a > .0f)
                {
                    color.a -= Time.deltaTime * 2.0f;
                }
                else
                {
                    destoryTimer = 1.5f;
                    startImgFadeOut = false;
                    yield break;

                }

                sp.color = color;


            }

            yield return null;
        }
    }




    public virtual void OnHeal(int heal) { }

    public abstract void OnEnable();
    public abstract void EnemySensor(); //������ ����

    public abstract void AttackDelay();

    public abstract void OnAttack();

    public abstract void OnDamage(int att, int knockBack = 0, bool criticalCheck = false);

    public abstract bool CriticalCheck();

    public abstract void CriticalAttack(Unit ctrl,string soundPath,string criticalSoundPath,string hitPath);
    public abstract void CriticalAttack(Tower ctrl, string soundPath, string criticalSoundPath, string hitPath);

    public void AddHpObserver(IHpObserver observer)
    {   //���� ��ü�� ������ ���� ���������� �߰��Ѵ�.
        observerList.Add(observer);
    }

    public void RemoveHpObserver(IHpObserver observer)
    {   //���� ��ü�� ������ �ȹ��� ���������� �����Ѵ�.
        observerList.Remove(observer);
    }

    public void NotifyToHpObserver()   //�������� �԰ų� ȸ���ϸ� ���������� ���� ü���� �����ֱ�
    {
        float hpPercent = hp / maxHp;
        //�����ϰ��ִ� �������鿡�� �������� ����
        foreach (IHpObserver obs in observerList)
            obs.Notified(hpPercent);       //���� ������ ü���ۼ�Ʈ�� �����ϰ� �ִ� �������鿡�� �������ش�.
        
    }
}
