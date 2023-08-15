using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private RectTransform[] rtTrs;
    Define.SubStage targetStage = Define.SubStage.Three;
    Define.SubStage curStage;
    [SerializeField] bool isGo = false;     //도착했는지 판단하는 변수
    [SerializeField] float speed = 5.0f;

    bool[] isDistance = new bool[3];
    bool[] isLeftDistance = new bool[3];


    Vector3 dir = Vector3.zero;
    Vector3 norDir = Vector3.zero;
    private Vector3 rightMoveScale = new Vector3(-1, 1, 1);
    private Vector3 leftMoveScale = new Vector3(1, 1, 1);

    public bool IsGo { get { return isGo; } }
    // Start is called before the first frame update
    void Start()
    {
        this.TryGetComponent(out anim);
        curStage = GlobalData.curStage;
        for(int i = 0; i < isDistance.Length; i++)
        {
            isDistance[i] = false;
            isLeftDistance[i] = false;
        }

        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMove();
    }


    void Init()
    {
        this.transform.position = rtTrs[(int)GlobalData.curStage].transform.position;
    }


    void PlayerMove()
    {
        //for(int ii = (int)GlobalData.curStage; ii < (int)targetStage; ii++)
        //{
        //    rtTrs[ii].localPosition
        //}


        if(isGo)
        {
            if (this.gameObject.transform.position.x < rtTrs[(int)targetStage].transform.position.x - 5.0f) //오른쪽으로 이동
            {
                anim.SetBool("Run", true);
                if(this.gameObject.transform.localScale != rightMoveScale)
                    this.gameObject.transform.localScale = rightMoveScale;

                if (gameObject.transform.position.x <= rtTrs[(int)Define.SubStage.Two].transform.position.x - 5.0f)
                {
                    if (!isDistance[(int)Define.SubStage.Two - 1])
                    {
                        isDistance[(int)Define.SubStage.Two - 1] = true;
                        dir = rtTrs[(int)Define.SubStage.Two].position - this.gameObject.transform.position;
                        norDir = dir.normalized;
                    }

                    this.gameObject.transform.position += dir * Time.deltaTime * speed;


                }
                else if (gameObject.transform.position.x <= rtTrs[(int)Define.SubStage.Three].transform.position.x - 5.0f)
                {

                    

                    if (!isDistance[(int)Define.SubStage.Three - 1])
                    {
                        isDistance[(int)Define.SubStage.Three - 1] = true;
                        dir = rtTrs[(int)Define.SubStage.Three].position - this.gameObject.transform.position;
                        norDir = dir.normalized;
                    }

                    this.gameObject.transform.position += dir * Time.deltaTime * speed;

                }
                else if (gameObject.transform.position.x <= rtTrs[(int)Define.SubStage.Boss].transform.position.x - 5.0f)
                {
                    if (!isDistance[(int)Define.SubStage.Boss - 1])
                    {
                        isDistance[(int)Define.SubStage.Boss - 1] = true;
                        dir = rtTrs[(int)Define.SubStage.Boss].position - this.gameObject.transform.position;
                        norDir = dir.normalized;
                    }

                    this.gameObject.transform.position += dir * Time.deltaTime * speed;

                }
      

      



            }
            else if(rtTrs[(int)targetStage].transform.position.x + 5.0f < this.gameObject.transform.position.x)  //왼쪽으로 이동!
            {
                anim.SetBool("Run", true);
                if (this.gameObject.transform.localScale != leftMoveScale)
                    this.gameObject.transform.localScale = leftMoveScale;

                if (gameObject.transform.position.x > rtTrs[(int)Define.SubStage.Three].transform.position.x + 5.0f)
                {

                    if (!isLeftDistance[(int)Define.SubStage.Three])
                    {
                        isLeftDistance[(int)Define.SubStage.Three] = true;
                        dir = rtTrs[(int)Define.SubStage.Three].position - this.gameObject.transform.position;
                        norDir = dir.normalized;
                    }

                    this.gameObject.transform.position += dir * Time.deltaTime * speed;

                }
                else if (gameObject.transform.position.x > rtTrs[(int)Define.SubStage.Two].transform.position.x + 5.0f)
                {

                    if (!isLeftDistance[(int)Define.SubStage.Two])
                    {
                        isLeftDistance[(int)Define.SubStage.Two] = true;
                        dir = rtTrs[(int)Define.SubStage.Two].position - this.gameObject.transform.position;
                        norDir = dir.normalized;
                    }

                    this.gameObject.transform.position += dir * Time.deltaTime * speed;

                }
                else if (gameObject.transform.position.x > rtTrs[(int)Define.SubStage.One].transform.position.x + 5.0f)
                {

                    if (!isLeftDistance[(int)Define.SubStage.One])
                    {
                        isLeftDistance[(int)Define.SubStage.One] = true;
                        dir = rtTrs[(int)Define.SubStage.One].position - this.gameObject.transform.position;
                        norDir = dir.normalized;
                    }

                    this.gameObject.transform.position += dir * Time.deltaTime * speed;

                }
            }

            else
            {
                if (isGo == true)
                {
                    isGo = false;
                    anim.SetBool("Run", false);
                    GlobalData.curStage = targetStage;
                    for(int ii = 0; ii < isDistance.Length; ii++)
                    {
                        isDistance[ii] = false;
                        isLeftDistance[ii] = false;
                    }
                }

            }


        }
        

    }






    public void SetTarget(Define.SubStage targetStage,bool isGo)
    {

        if (this.isGo)
            return;


        this.targetStage = targetStage;
        this.isGo = isGo;
    }
}
