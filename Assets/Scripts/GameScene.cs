using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameScene : BaseScene
{
    //게임씬에 들어왔을때 씬에서 필요한것들을 처음에 갱신해주는 역할
    [SerializeField] private MonsterSpawn monSpawn;

    private PlayableDirector playable;
    private MoneyCost moneyCost;


    GameObject obj;
    UnitNode unitnode;
    float speed = 1.0f;

    public MoneyCost MoneyCost { get { return moneyCost; } }

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
        TryGetComponent(out moneyCost);


    }

    // Update is called once per frame
    void Update()
    {
        if (playable.state == PlayState.Playing)  //타임라인이 작동중이면 리턴
            return;

        if (Managers.UI.GetSceneUI<UI_GamePlay>() == null)
            return;

        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCoolTime(speed);


        //if (UI_GamePlay.gameQueue.Count > 0)
        //{
        //    Managers.UI.GetSceneUI<UI_GamePlay>().Summon(obj, spawnPos);
        //    //go.TryGetComponent(out unitnode);
        //    //unitnode.CoolCheck = true;  //해당 유닛이 소환이 되면 쿨타임 추가 (true면 쿨온)
        //}


        moneyCost.CostCoolTimer();
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





    public override void Clear()
    {

    }



}
