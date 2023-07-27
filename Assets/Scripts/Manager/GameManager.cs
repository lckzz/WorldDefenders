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
    //테스트용으로 게임매니저에 UI연결해서 동작만 확인
    public static GameManager instance;
    //나중에 UI따로 관리해서 게임매니저에 연결

    public GameState state = GameState.GamePlaying;
    public Button uiUnitSword;
    public Button uiUnitBow;
    public Button uiAttackBtn;
    public Transform[] spawnPos;
    public Transform[] monsterSpawnPos;
    public GameObject[] monsters;
    public SpriteRenderer[] sprends;
    public float timerSec = 0;

    public GameObject ui_GameResult;
    [SerializeField]
    float monsterSpawnTimer = 10.5f;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


        if (uiUnitSword != null)
            uiUnitSword.onClick.AddListener(UiUnitSword);
        if (uiUnitBow != null)
            uiUnitBow.onClick.AddListener(UiUnitBow);
        if (uiAttackBtn != null)
            uiAttackBtn.onClick.AddListener(UiAttack);

        
    }

    // Update is called once per frame
    void Update()
    {
        if(monsterSpawnTimer > 0.0f)
        {
            monsterSpawnTimer -= Time.deltaTime;
            if(monsterSpawnTimer <= 0.0f)
            {
                monsterSpawnTimer = 0.0f;
                int ran = Random.Range(0, 2);
                int ranPos = Random.Range(0, 3);
                GameObject obj = Instantiate(monsters[ran], monsterSpawnPos[ranPos].position,Quaternion.identity);
                //obj.transform.position = monsterSpawnPos[ranPos].position;
                //switch(ranPos)
                //{
                //    case 0:
                //     SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
                //        sp.sortingOrder = 8;
                //        break;
                //    case 1:
                //        SpriteRenderer sp1 = obj.GetComponent<SpriteRenderer>();
                //        sp1.sortingOrder = 9;
                //        break;
                //    case 2:
                //        SpriteRenderer sp2 = obj.GetComponent<SpriteRenderer>();
                //        sp2.sortingOrder = 10;
                //        break;
                //}
                monsterSpawnTimer = 8.0f;


            }
        }

        if(state == GameState.GamePlaying)
        {
            timerSec += Time.deltaTime;
        }

        if (state == GameState.GameFail || state == GameState.GameVictory)
        {
            if(!ui_GameResult.activeSelf)
                ui_GameResult.SetActive(true);

        }




    }


    void UiUnitSword()  //누르면 검 유닛생간
    {
        Debug.Log("검 소환");
        GameObject obj = Resources.Load<GameObject>("Prefab/Unit/KnifeUnit");
        GameObject ob = Instantiate(obj);
        SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();

        int ran = Random.Range(0, 3);
        ob.transform.position = spawnPos[ran].position;
        switch (ran)
        {
            case 0:
                sp.sortingOrder = 8;
                break;
            case 1:
                sp.sortingOrder = 9;
                break;
            case 2:
                sp.sortingOrder = 10;
                break;
        }
    }

    void UiUnitBow()  //누르면 활 유닛 생산
    {
        Debug.Log("궁 소환");
        GameObject obj = Resources.Load<GameObject>("Prefab/Unit/BowUnit");
        GameObject ob = Instantiate(obj);
        SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();

        int ran = Random.Range(0, 3);
        ob.transform.position = spawnPos[ran].position;
        switch (ran)
        {
            case 0:
                sp.sortingOrder = 8;
                break;
            case 1:
                sp.sortingOrder = 9;
                break;
            case 2:
                sp.sortingOrder = 10;
                break;
        }
    }

    void UiAttack()
    {
        Debug.Log("활 발싸");
        //player.AttackWait();
        
    }


}
