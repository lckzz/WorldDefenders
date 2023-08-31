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

        //�����ϴ�
        groundMin.x = this.transform.position.x - a_GroundHalfSize.x;
        groundMin.y = this.transform.position.y - a_GroundHalfSize.y;
        //�������
        groundMin2.x = this.transform.position.x - a_GroundHalfSize.x;
        groundMin2.y = this.transform.position.y + a_GroundHalfSize.y;
        //�������
        groundMax.x = this.transform.position.x + a_GroundHalfSize.x;
        groundMax.y = this.transform.position.y + a_GroundHalfSize.y;
        //�����ϴ�
        groundMax2.x = this.transform.position.x + a_GroundHalfSize.x;
        groundMax2.y = this.transform.position.y - a_GroundHalfSize.y;
    }

    // Update is called once per frame
    void Update()
    {
        a_GroundHalfSize.x = this.transform.localScale.x / 2.0f;
        a_GroundHalfSize.y = this.transform.localScale.y / 2.0f;

        //�����ϴ�
        groundMin.x = this.transform.position.x - a_GroundHalfSize.x;
        groundMin.y = this.transform.position.y - a_GroundHalfSize.y;
        //�������
        groundMin2.x = this.transform.position.x - a_GroundHalfSize.x;
        groundMin2.y = this.transform.position.y + a_GroundHalfSize.y;
        //�������
        groundMax.x = this.transform.position.x + a_GroundHalfSize.x;
        groundMax.y = this.transform.position.y + a_GroundHalfSize.y;
        //�����ϴ�
        groundMax2.x = this.transform.position.x + a_GroundHalfSize.x;
        groundMax2.y = this.transform.position.y - a_GroundHalfSize.y;
        //������ܿ��� ������ܱ����� ��
        Debug.DrawLine(groundMin2, groundMax, Color.black);
        //������ܿ��� �����ϴܱ����� ��
        Debug.DrawLine(groundMax, groundMax2, Color.black);
        //�����ϴܿ��� �����ϴܱ����� ��
        Debug.DrawLine(groundMax2, groundMin, Color.black);
        //�����ϴܿ��� ������ܱ����� ��
        Debug.DrawLine(groundMin, groundMin2, Color.black);


    }
}
