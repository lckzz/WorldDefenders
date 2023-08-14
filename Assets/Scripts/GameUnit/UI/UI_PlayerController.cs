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

    bool isDistance = false;

    Vector3 dir = Vector3.zero;
    Vector3 norDir = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        this.TryGetComponent(out anim);
        curStage = GlobalData.curStage;
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMove();
    }


    void PlayerMove()
    {
        //for(int ii = (int)GlobalData.curStage; ii < (int)targetStage; ii++)
        //{
        //    rtTrs[ii].localPosition
        //}


        if(isGo)
        {
            if ((int)curStage < (int)targetStage) //오른쪽으로 이동
            {
                if ((int)curStage + 1 == (int)targetStage)
                {
                    //다음 스테이지가 목표스테이지라면
                    this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, rtTrs[(int)targetStage].position, Time.deltaTime * speed);
                    if (this.gameObject.transform.position.x >= rtTrs[(int)targetStage].position.x - 0.5f)
                    {
                        isGo = false;
                        curStage = targetStage;
                    }

                }

                if ((int)curStage + 2 == (int)targetStage)        //다음스테이지가 목표스테이지가 아니면
                {
                    if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 1].position.x - 5.0f)
                    {
                        if (!isDistance)
                        {
                            isDistance = true;
                            dir = rtTrs[(int)curStage + 1].position - this.gameObject.transform.position;
                            norDir = dir.normalized;
                        }

                        this.gameObject.transform.position += dir * Time.deltaTime * speed;

                    }
                    else
                    {
                        if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 2].position.x - 5.0f)
                        {
                            if (isDistance)
                            {
                                isDistance = false;
                                dir = rtTrs[(int)curStage + 2].position - this.gameObject.transform.position;
                                norDir = dir.normalized;
                            }

                            this.gameObject.transform.position += dir * Time.deltaTime * speed;

                        }

                        else
                        {
                            curStage = targetStage;
                            isGo = false;

                        }

                    }

                }

                if ((int)curStage + 3 == (int)targetStage)        //목표스테이지가 3단계 건너있을때
                {
                    if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 1].position.x - 5.0f)
                    {
                        if (!isDistance)
                        {
                            isDistance = true;
                            dir = rtTrs[(int)curStage + 1].position - this.gameObject.transform.position;
                            norDir = dir.normalized;
                        }

                        this.gameObject.transform.position += dir * Time.deltaTime * speed;

                    }
                    else if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 2].position.x - 5.0f)
                    {
                        if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 2].position.x - 5.0f)
                        {
                            if (isDistance)
                            {
                                isDistance = false;
                                dir = rtTrs[(int)curStage + 2].position - this.gameObject.transform.position;
                                norDir = dir.normalized;
                            }

                            this.gameObject.transform.position += dir * Time.deltaTime * speed;

                        }
                    }
                    else
                    {
                        if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 3].position.x - 5.0f)
                        {
                            if (!isDistance)
                            {
                                isDistance = true;
                                dir = rtTrs[(int)GlobalData.curStage + 3].position - this.gameObject.transform.position;
                                norDir = dir.normalized;
                            }

                            this.gameObject.transform.position += dir * Time.deltaTime * speed;

                        }
                        else
                        {
                            curStage = targetStage;
                            isGo = false;

                        }

                    }

                }



            }
            else   //왼쪽으로 이동!
            {
                if ((int)curStage - 1 == (int)targetStage)
                {
                    //다음 스테이지가 목표스테이지라면
                    this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, rtTrs[(int)targetStage].position, Time.deltaTime * speed);
                    if (this.gameObject.transform.position.x >= rtTrs[(int)targetStage].position.x - 0.5f)
                    {
                        isGo = false;
                        curStage = targetStage;
                    }

                }

                if ((int)curStage - 2 == (int)targetStage)        //다음스테이지가 목표스테이지가 아니면
                {
                    if (this.gameObject.transform.position.x + 5.0f >= rtTrs[(int)curStage - 1].position.x)
                    {
                        if (!isDistance)
                        {
                            isDistance = true;
                            dir = rtTrs[(int)curStage - 1].position - this.gameObject.transform.position;
                            norDir = dir.normalized;
                        }

                        this.gameObject.transform.position += dir * Time.deltaTime * speed;

                    }
                    else
                    {
                        if (this.gameObject.transform.position.x + 5.0f >= rtTrs[(int)curStage + 2].position.x)
                        {
                            if (isDistance)
                            {
                                isDistance = false;
                                dir = rtTrs[(int)curStage - 2].position - this.gameObject.transform.position;
                                norDir = dir.normalized;
                            }

                            this.gameObject.transform.position += dir * Time.deltaTime * speed;

                        }

                        else
                        {
                            curStage = targetStage;
                            isGo = false;

                        }

                    }

                }

                if ((int)curStage + 3 == (int)targetStage)        //목표스테이지가 3단계 건너있을때
                {
                    if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 1].position.x - 5.0f)
                    {
                        if (!isDistance)
                        {
                            isDistance = true;
                            dir = rtTrs[(int)curStage + 1].position - this.gameObject.transform.position;
                            norDir = dir.normalized;
                        }

                        this.gameObject.transform.position += dir * Time.deltaTime * speed;

                    }
                    else if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 2].position.x - 5.0f)
                    {
                        if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 2].position.x - 5.0f)
                        {
                            if (isDistance)
                            {
                                isDistance = false;
                                dir = rtTrs[(int)curStage + 2].position - this.gameObject.transform.position;
                                norDir = dir.normalized;
                            }

                            this.gameObject.transform.position += dir * Time.deltaTime * speed;

                        }
                    }
                    else
                    {
                        if (this.gameObject.transform.position.x <= rtTrs[(int)curStage + 3].position.x - 5.0f)
                        {
                            if (!isDistance)
                            {
                                isDistance = true;
                                dir = rtTrs[(int)GlobalData.curStage + 3].position - this.gameObject.transform.position;
                                norDir = dir.normalized;
                            }

                            this.gameObject.transform.position += dir * Time.deltaTime * speed;

                        }
                        else
                        {
                            curStage = targetStage;
                            isGo = false;

                        }

                    }

                }

            }


        }
        

    }


    public void SetTarget(Define.SubStage targetStage,bool isGo)
    {
        this.targetStage = targetStage;
        this.isGo = isGo;
    }
}
