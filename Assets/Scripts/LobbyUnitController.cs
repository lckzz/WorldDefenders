using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUnitController : MonoBehaviour
{


    private Define.LobbyUnitState _lobbyState;
    public Define.LobbyUnitState LobbyState { get { return _lobbyState; } }


    [SerializeField] private GameObject selectObj;
    private Animator anim;
    private Vector3 goalPos;
    private float speed = 5.0f;

    private bool isRun = false;
    private bool isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        selectObj = this.gameObject.transform.GetChild(0).gameObject;
        TryGetComponent(out anim);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_lobbyState)
        {
            case Define.LobbyUnitState.Idle:

                break;

            case Define.LobbyUnitState.Run:
                UnitMove();
                break;

            case Define.LobbyUnitState.Attack:
                UnitAttack();
                break;



        }
    }



    void UnitMove()
    {
        Vector3 dir = goalPos - this.transform.position;
        float distance = dir.sqrMagnitude;
        if(distance < 0.1f)
        {
            SetState(Define.LobbyUnitState.Idle);
            return;
        }

        Vector3 dirVec = dir.normalized;
        transform.position += (dirVec * speed * Time.deltaTime);

        if(dirVec.x > 0)
            this.transform.localScale = new Vector3(1, 1, 1);
        else
            this.transform.localScale = new Vector3(-1, 1, 1);
    }

    void UnitAttack()
    {
        if (anim != null)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && 
                (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack")))
                SetState(Define.LobbyUnitState.Idle);
        }
    }

    public void SelectUnitCircleOnOff(bool isOn)
    {
        selectObj?.SetActive(isOn);
    }

    public void SetMove(Vector3 pos)
    {
        goalPos = pos;
        SetState(Define.LobbyUnitState.Run);

    }

    public void SetAttackState()
    {
        SetState(Define.LobbyUnitState.Attack);
    }


    public void SetState(Define.LobbyUnitState state)
    {
        _lobbyState = state;

        switch(_lobbyState)
        {
            case Define.LobbyUnitState.Idle:

                if(isRun)
                {
                    isRun = false;
                    anim.SetBool("Run", isRun);
                }
                if(isAttack)
                {
                    isAttack = false;
                    anim.SetBool("Attack", isAttack);
                }

                break;
            case Define.LobbyUnitState.Run:
                if (!isRun)
                {
                    isRun = true;
                    anim.SetBool("Run", isRun);
                }
                if (isAttack)
                {
                    isAttack = false;
                    anim.SetBool("Attack", isAttack);
                }
                break;
            case Define.LobbyUnitState.Attack:
                if (isRun)
                {
                    isRun = false;
                    anim.SetBool("Run", isRun);
                }
                if (!isAttack)
                {
                    isAttack = true;
                    anim.SetBool("Attack", isAttack);
                }
                break;



        }
    }
}
