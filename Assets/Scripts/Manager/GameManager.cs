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
    //현재 인게임에 대한 정보를 관리하는 매니저이다.
    //게임재화 코스트 관리 몬스터 스폰 
    public static GameManager instance;
    //테스트용으로 게임매니저에 UI연결해서 동작만 확인
    //나중에 UI따로 관리해서 게임매니저에 연결

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
