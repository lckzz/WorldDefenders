using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraCtrl : MonoBehaviour
{
    GameObject moveObj;
    Vector3 newPostion = Vector3.zero;
    float smoothTime = 0.05f;
    float xVelocity = 0.0f;
    float yVelocity = 0.0f;


    //카메라 제한
    [HideInInspector] public Vector3 m_GroundMin = Vector3.zero;    //지형의 하단
    [HideInInspector] public Vector3 m_GroundMax = Vector3.zero;    //지형의 우측상단

    [HideInInspector] public Vector3 m_CamWMin = Vector3.zero;
    [HideInInspector] public Vector3 m_CamWMax = Vector3.zero;
    Vector3 m_ScWdHalf = Vector3.zero;

    float a_LmtBdLeft = 0;
    float a_LmtBdTop = 0;
    float a_LmtBdRight = 0;
    float a_LmtBdBottom = 0;
    //카메라 제한



    //카메라 흔들기
    [HideInInspector] public float ShakeAmount;
    float ShakeTime;
    Vector3 initPosition;

    // Start is called before the first frame update
    void Start()
    {
        moveObj = GameObject.Find("CameraMoveObj");
        GameObject Groundobj = GameObject.Find("GroundObj");

        Vector3 a_GrdHalfSize = Vector3.zero;
        a_GrdHalfSize.x = Groundobj.transform.localScale.x / 2.0f;
        a_GrdHalfSize.y = Groundobj.transform.localScale.y / 2.0f;

        //좌측하단
        m_GroundMin.x = Groundobj.transform.position.x - a_GrdHalfSize.x;
        m_GroundMin.y = Groundobj.transform.position.y - a_GrdHalfSize.y;
        //우측상단
        m_GroundMax.x = Groundobj.transform.position.x + a_GrdHalfSize.x;
        m_GroundMax.y = Groundobj.transform.position.y + a_GrdHalfSize.y;

        initPosition = this.transform.position;

    }


    // Update is called once per frame
    void Update()
    {
        //카메라 화면 좌측하단 코너의 월드 좌표
        m_CamWMin = Camera.main.ViewportToWorldPoint(Vector3.zero);
        //카메라 화면 우측상단 코너의 월드 좌표
        m_CamWMax = Camera.main.ViewportToWorldPoint(Vector3.zero);



        //Vector3 playerPos = animal.transform.position;
        //transform.position = new Vector3(playerPos.x, playerPos.y + 2.0f, transform.position.z);
    }


    private void LateUpdate()
    {
        if (moveObj == null)
            return;

        newPostion = transform.position;
        newPostion.x = Mathf.SmoothDamp(transform.position.x, moveObj.transform.position.x,
            ref xVelocity, smoothTime);
        newPostion.y = Mathf.SmoothDamp(transform.position.y, moveObj.transform.position.y,
            ref yVelocity, smoothTime);

        //카메라 화면 좌측하단 코너의 월드 좌표
        m_CamWMin = Camera.main.ViewportToWorldPoint(Vector3.zero);
        //카메라 화면 우측상단 코너의 월드 좌표
        m_CamWMax = Camera.main.ViewportToWorldPoint(Vector3.zero);

        if (ShakeTime <= 0.0f)
        {
            m_ScWdHalf.x = (m_CamWMax.x - m_CamWMin.x) / 2.0f;
            m_ScWdHalf.y = (m_CamWMax.y - m_CamWMin.y) / 2.0f;
            a_LmtBdLeft = m_GroundMin.x + 11.5f + m_ScWdHalf.x;
            a_LmtBdTop = m_GroundMax.y - 6.5f - m_ScWdHalf.y;
            a_LmtBdRight = m_GroundMax.x - 10.5f - m_ScWdHalf.x;
            a_LmtBdBottom = m_GroundMax.y - 9.0f - m_ScWdHalf.y;

            //Debug.Log(a_LmtBdLeft);
            //Debug.Log(a_LmtBdTop);
            if (newPostion.x < a_LmtBdLeft)
                newPostion.x = a_LmtBdLeft;

            if (a_LmtBdRight < newPostion.x)
                newPostion.x = a_LmtBdRight;

            if (a_LmtBdBottom > newPostion.y)
                newPostion.y = a_LmtBdBottom;

            if (a_LmtBdTop < newPostion.y)
                newPostion.y = a_LmtBdTop;


            transform.position = newPostion;

        }

    }


    public void VibrateForTime(float time)
    {
        ShakeTime = time;
    }
}
