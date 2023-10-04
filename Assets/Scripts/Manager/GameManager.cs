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

    public static List<UnitNode> unitList = new List<UnitNode>();
    float speed = 1.0f;
    float curCost = .0f;
    float maxCost = 500.0f;
    float costCoolTime = 1.0f;
    [SerializeField]
    public GameState state = GameState.GamePlaying;
    public Button uiUnitSword;
    public Button uiUnitBow;
    public Button uiAttackBtn;
    public Transform[] spawnPos;
    public Transform[] monsterSpawnPos;
    public GameObject[] monsters;
    public GameObject eliteWarrior;
    public SpriteRenderer[] sprends;
    public GameObject sceneEndPanel;
    public float timerSec = 0;

    bool gameSet = false;
    bool bossOn = false;
    bool eventOn = false;    //���������̺�Ʈ
    bool gameEnd = false;

    public GameObject ui_GameResult;
    [SerializeField]
    float monsterSpawn = 5.5f;
    [SerializeField] float spawnTimer = 8.5f;

    public GameState State { get { return state; } set { state = value; } }
    public float CurCost { get { return curCost; } set { curCost = value; } }
    public float MaxCost { get { return maxCost; } set { maxCost = value; } }
    public bool GameSet { get { return gameSet; } set { gameSet = value; } }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }


        Debug.Log(GlobalData.g_SlotUnitClass.Count);
        //if (uiUnitSword != null)
        //    uiUnitSword.onClick.AddListener(UiUnitSword);
        //if (uiUnitBow != null)
        //    uiUnitBow.onClick.AddListener(UiUnitBow);
        //if (uiAttackBtn != null)
        //    uiAttackBtn.onClick.AddListener(UiAttack);

        
    }

    // Update is called once per frame
    void Update()
    {

        if(state == GameState.GamePlaying)
        {
            if (bossOn == false)
            {
                if (timerSec > 30.0f)
                {
                    bossOn = true;
                    int ranPos = Random.Range(0, 3);
                    GameObject obj = Managers.Resource.Instantiate(eliteWarrior, monsterSpawnPos[ranPos].position);
                }

               
            }

            if(eventOn == false)
            {
                if(timerSec > 15.0f && timerSec <= 20.0f)
                {
                    eventOn = true;

                    spawnTimer = 3.0f;
                }
            }


            if(eventOn == true)
            {
                if(timerSec > 50.0f)
                {
                    eventOn = false;
                    spawnTimer = 8.5f;
                }
            }


            if (monsterSpawn > 0.0f)                       //���� ������ ���� �ļ� �ؾ���
            {
                monsterSpawn -= Time.deltaTime;
                if (monsterSpawn <= 0.0f)
                {
                    monsterSpawn = 0.0f;
                    int ran = Random.Range(0, 2);
                    int ranPos = Random.Range(0, 3);
                    GameObject obj = Managers.Resource.Instantiate(monsters[ran], monsterSpawnPos[ranPos].position, Quaternion.identity);
                    obj.transform.position = monsterSpawnPos[ranPos].position;
                    monsterSpawn = spawnTimer;


                }
            }
        }
        




        if (state == GameState.GamePlaying)
        {
            timerSec += Time.deltaTime;
        }

        if (state == GameState.GameFail || state == GameState.GameVictory)
        {
            if(gameEnd == false)
            {
                gameEnd = true;
                Managers.UI.ShowPopUp<UI_GameResult>();
                Managers.UI.OnOffSceneUI<UI_GamePlay>(false);

            }

        }




    }


    public void CostCoolTimer(float coolTime, float cost)
    {

        if (costCoolTime > .0f)
        {
            costCoolTime -= Time.deltaTime;
            if (costCoolTime <= .0f)
            {
                costCoolTime = coolTime;
                curCost += cost;
                Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCost(curCost);

            }
        }
    }

    public void SceneEndOn()
    {
        sceneEndPanel.SetActive(true);
    }


    public bool CostCheck(float curCost, float unitCost)
    {
        if (curCost - unitCost < .0f) //0���� �۴ٸ�
            return false;

        return true;

    }

    public float CostUse(float curCost,float unitCost)
    {
        curCost -= unitCost;

        return curCost;
    }


}
