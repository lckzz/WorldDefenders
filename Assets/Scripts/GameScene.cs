using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameScene : BaseScene
{
    //게임씬에 들어왔을때 씬에서 필요한것들을 처음에 갱신해주는 역할
    //float speed = 1.0f;
    //float curCost = .0f;
    //float maxCost = 500.0f;
    //float costCoolTime = 1.0f;
    [SerializeField] private MonsterSpawn monSpawn;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] float coolTime = 4.5f;
    private PlayableDirector playable;
    float curCost = .0f;
    float maxCost = 500.0f;
    float costCoolTime = 4.5f;
    GameObject obj;
    UnitNode unitnode;
    float speed = 1.0f;

    public float CurCost { get { return curCost; } set { curCost = value; } }
    public float MaxCost { get { return maxCost; } set { maxCost = value; } }


    // Start is called before the first frame update
    void Start()
    {

        Init();
        Managers.UI.CloseAllPopUpUI();
        Dictionary<int,TowerStat> dict = Managers.Data.towerDict;


    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.BattleStage_Field;
        Managers.Sound.Play("BGM/GameBGM", Define.Sound.BGM);
        TryGetComponent(out playable);
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Managers.UI.GetSceneUI<UI_GamePlay>() == null)
            return;

        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCoolTime(speed);
        CostCoolTimer(costCoolTime, 30.0f);

        if (UI_GamePlay.gameQueue.Count > 0)
        {
            Managers.UI.GetSceneUI<UI_GamePlay>().Summon(obj, spawnPos);
            //go.TryGetComponent(out unitnode);
            //unitnode.CoolCheck = true;  //해당 유닛이 소환이 되면 쿨타임 추가 (true면 쿨온)
        }

        monSpawn.NormalMonsterSpawn();

    }

    public void UiOnOff(bool isOn)
    {
        if (Managers.UI.GetSceneUI<UI_GamePlay>() != null)
        {
            Managers.UI.GetSceneUI<UI_GamePlay>().gameObject.SetActive(isOn);
            Time.timeScale = 1.0f;
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


    public bool CostCheck(float curCost, float unitCost)
    {
        if (curCost - unitCost < .0f) //0보다 작다면
            return false;

        return true;

    }

    public float CostUse(float curCost, float unitCost)
    {
        curCost -= unitCost;

        return curCost;
    }


    public override void Clear()
    {

    }



}
