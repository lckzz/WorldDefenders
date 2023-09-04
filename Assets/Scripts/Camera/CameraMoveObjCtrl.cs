using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMoveObjCtrl : MonoBehaviour
{
    GroundCheck groundCheck;
    GameObject moveObj;

    float moveSpeed = 20.0f;
    float x = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        moveObj = GameObject.Find("CameraMoveObj");

        GameObject.Find("GroundObj").TryGetComponent<GroundCheck>(out groundCheck);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            x = Input.GetAxis("Mouse X");
            if (x > 0) //왼쪽으로 드래그하면 오른쪽으로
            {
                Vector3 nor = new Vector3(x, .0f, .0f);
                Vector3 nor1 = nor.normalized;
                moveObj.transform.position += (nor1 * moveSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 nor = new Vector3(x, .0f, .0f);
                Vector3 nor1 = nor.normalized;
                moveObj.transform.position += (nor1 * moveSpeed * Time.deltaTime);
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (x != 0.0f)
                x = 0.0f;
        }


        





        if (groundCheck.GroundMin.x + 3.5f > this.transform.position.x)     //플레이어가 배경의 왼쪽으로 벗어나려고하면
        {
            Vector3 pos = this.transform.position;
            pos.x = groundCheck.GroundMin.x + 3.5f;
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

        if (groundCheck.GroundMax.x - 3.5f < this.transform.position.x)     //플레이어가 오른쪽으로 벗어나려고 하면
        {
            Vector3 pos = this.transform.position;
            pos.x = groundCheck.GroundMax.x - 3.5f;
            this.transform.position = pos;
        }
    }
}
