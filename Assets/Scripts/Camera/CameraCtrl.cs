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

    float moveSpeed = 20.0f;
    float x = 0.0f;
    float width;
    float height;

    GroundCheck groundCheck;
    CameraShake shake;


    public CameraShake Shake { get { return shake; } }

    //카메라 흔들기
    [HideInInspector] public float ShakeAmount;
    float ShakeTime;
    Vector3 initPosition;

    // Start is called before the first frame update
    void Start()
    {
        moveObj = GameObject.Find("CameraMoveObj");
        GameObject Groundobj = GameObject.Find("GroundObj");
        Groundobj.TryGetComponent<GroundCheck>(out groundCheck);
        Debug.Log(TryGetComponent(out shake));
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
        height = Camera.main.orthographicSize;                  //Size는 높이의 절반값
        //width = height * Screen.width / Screen.height;
        width = height * Camera.main.aspect;


    }


    // Update is called once per frame
    void Update()
    {
        


        //카메라 화면 좌측하단 코너의 월드 좌표
        m_CamWMin = Camera.main.ViewportToWorldPoint(Vector3.zero);
        //카메라 화면 우측상단 코너의 월드 좌표
        m_CamWMax = Camera.main.ViewportToWorldPoint(Vector3.zero);


        if (Managers.UI.GetSceneUI<UI_GamePlay>() != null)
        {
            if (Managers.UI.GetSceneUI<UI_GamePlay>().RightBtnCheck)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }

            else if (Managers.UI.GetSceneUI<UI_GamePlay>().LeftBtnCheck)
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

    

        #region 카메라 움직임 제한범위
        if (groundCheck.GroundMin.x + width + 0.3f > this.transform.position.x)     //플레이어가 배경의 왼쪽으로 벗어나려고하면
        {
            Vector3 pos = this.transform.position;
            pos.x = groundCheck.GroundMin.x + width + 0.3f;
            this.transform.position = pos;
        }

        if (groundCheck.GroundMin.y + 0.7f > this.transform.position.y)        //플레이어의 y축좌표가 좌측하단의 y축보다 작아지려하면
        {
            Vector3 pos = this.transform.position;
            pos.y = groundCheck.GroundMin.y + 0.7f;
            this.transform.position = pos;
        }

        if (groundCheck.GroundMin2.y < this.transform.position.y)           //플레이어의 y축좌표가 좌측상단의 y축보다 커지려하면
        {
            Vector3 pos = this.transform.position;
            pos.y = groundCheck.GroundMin2.y;
            this.transform.position = pos;
        }

        if (groundCheck.GroundMax.x - width < this.transform.position.x)     //플레이어가 오른쪽으로 벗어나려고 하면
        {
            Vector3 pos = this.transform.position;
            pos.x = groundCheck.GroundMax.x - width;
            this.transform.position = pos;
        }

        #endregion
        //Vector3 playerPos = animal.transform.position;
        //transform.position = new Vector3(playerPos.x, playerPos.y + 2.0f, transform.position.z);
    }


    private void LateUpdate()
    {
        if (moveObj == null)
            return;

        newPostion = transform.position;
        //newPostion.x = Mathf.SmoothDamp(transform.position.x, moveObj.transform.position.x,
        //    ref xVelocity, smoothTime);
        //newPostion.y = Mathf.SmoothDamp(transform.position.y, moveObj.transform.position.y,
        //    ref yVelocity, smoothTime);

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


    public void ShakeCamera(float shakeTime, float shakeIntensity)
    {
        //Shake.OnShakeCamera(shakeTime, shakeIntensity);
    }
}
