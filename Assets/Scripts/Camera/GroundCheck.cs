using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    Vector3 groundMin = Vector3.zero;
    Vector3 groundMax = Vector3.zero;
    Vector3 groundMin2 = Vector3.zero;
    Vector3 groundMax2 = Vector3.zero;
    float posx = 0.0f;
    float posy = 0.0f;
    Vector3 a_GroundHalfSize = Vector3.zero;


    public Vector3 GroundMin { get { return groundMin; } }
    public Vector3 GroundMax { get { return groundMax; } }
    public Vector3 GroundMin2 { get { return groundMin2; } }
    public Vector3 GroundMax2 { get { return groundMax2; } }

    // Start is called before the first frame update
    void Start()
    {

        a_GroundHalfSize.x = this.transform.localScale.x / 2.0f;
        a_GroundHalfSize.y = this.transform.localScale.y / 2.0f;

        //좌측하단
        groundMin.x = this.transform.position.x - a_GroundHalfSize.x;
        groundMin.y = this.transform.position.y - a_GroundHalfSize.y;
        //좌측상단
        groundMin2.x = this.transform.position.x - a_GroundHalfSize.x;
        groundMin2.y = this.transform.position.y + a_GroundHalfSize.y;
        //우측상단
        groundMax.x = this.transform.position.x + a_GroundHalfSize.x;
        groundMax.y = this.transform.position.y + a_GroundHalfSize.y;
        //우측하단
        groundMax2.x = this.transform.position.x + a_GroundHalfSize.x;
        groundMax2.y = this.transform.position.y - a_GroundHalfSize.y;
    }

    // Update is called once per frame
    void Update()
    {
        a_GroundHalfSize.x = this.transform.localScale.x / 2.0f;
        a_GroundHalfSize.y = this.transform.localScale.y / 2.0f;

        //좌측하단
        groundMin.x = this.transform.position.x - a_GroundHalfSize.x;
        groundMin.y = this.transform.position.y - a_GroundHalfSize.y;
        //좌측상단
        groundMin2.x = this.transform.position.x - a_GroundHalfSize.x;
        groundMin2.y = this.transform.position.y + a_GroundHalfSize.y;
        //우측상단
        groundMax.x = this.transform.position.x + a_GroundHalfSize.x;
        groundMax.y = this.transform.position.y + a_GroundHalfSize.y;
        //우측하단
        groundMax2.x = this.transform.position.x + a_GroundHalfSize.x;
        groundMax2.y = this.transform.position.y - a_GroundHalfSize.y;
        //좌측상단에서 우측상단까지의 선
        Debug.DrawLine(groundMin2, groundMax, Color.black);
        //우측상단에서 우측하단까지의 선
        Debug.DrawLine(groundMax, groundMax2, Color.black);
        //우측하단에서 좌측하단까지의 선
        Debug.DrawLine(groundMax2, groundMin, Color.black);
        //좌측하단에서 좌측상단까지의 선
        Debug.DrawLine(groundMin, groundMin2, Color.black);


    }
}
