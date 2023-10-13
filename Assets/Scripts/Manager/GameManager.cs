using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum GameState
{
    GamePlaying,
    GameFail,
    GameVictory
}

public class GameManager : MonoBehaviour
{
    //���� �ΰ��ӿ� ���� ������ �����ϴ� �Ŵ����̴�.
    //������ȭ �ڽ�Ʈ ���� ���� ���� 
    public static GameManager instance;
    //�׽�Ʈ������ ���ӸŴ����� UI�����ؼ� ���۸� Ȯ��
    //���߿� UI���� �����ؼ� ���ӸŴ����� ����

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }



        
    }

    // Update is called once per frame
    void Update()
    {

      


    }


    

}
