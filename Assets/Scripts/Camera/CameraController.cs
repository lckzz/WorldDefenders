using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Vector2 mapSize = new Vector2(0, 0);
    [SerializeField]
    Vector3 cameraPos;

    [SerializeField]
    Vector2 centor;

    [SerializeField]
    float cameraMoveSpeed;
    float height;
    float width;

    float moveSpeed = 50.0f;
    float x = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        height = Camera.main.orthographicSize;                  //Size는 높이의 절반값
        //width = height * Screen.width / Screen.height;
        width = height * Camera.main.aspect;
    }


    private void FixedUpdate()
    {
        LimitCameraArea();
        
    }

    void LimitCameraArea()
    {
        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + centor.x, lx + centor.x);
        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + centor.y, ly + centor.y);
        //Debug.Log($"현재 카메라 위치{transform.position} 왼쪽x상한치{Screen}");

        transform.position = new Vector3(clampX, clampY, -10f);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(centor, mapSize * 2);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            x = Input.GetAxis("Mouse X");
            if(x > 0) //왼쪽으로 드래그하면 오른쪽으로
            {
                Vector3 nor = new Vector3(x, .0f, .0f);
                Vector3 nor1 = nor.normalized;
                Camera.main.transform.position += (nor1 * moveSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 nor = new Vector3(x, .0f, .0f);
                Vector3 nor1 = nor.normalized;
                Camera.main.transform.position += (nor1 * moveSpeed * Time.deltaTime);
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(x != 0.0f)
                x = 0.0f;
        }

        if(Input.GetKey(KeyCode.A))
            Camera.main.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        
        else if(Input.GetKey(KeyCode.D))
            Camera.main.transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        x = Input.GetAxis("Horizontal");
        if (x > 0)
            Camera.main.transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        else if (x < 0)
            Camera.main.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        else
            x = 0;

    }
}
