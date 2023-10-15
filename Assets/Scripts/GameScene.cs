using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameScene : BaseScene
{
    //게임씬에 들어왔을때 씬에서 필요한것들을 처음에 갱신해주는 역할

    private PlayableDirector playable;

    GameObject obj;           //유닛소환할때 큐에서 게임오브젝트를 받아줄 변수
    [SerializeField] Transform unitParentTr;
    Transform[] unitSpawnTr = new Transform[3];
    [SerializeField] Transform monsterParentTr;
    Define.MonsterSpawnType monSpawnType = Define.MonsterSpawnType.Normal;      //이벤트에서 관리한값을 받아서 적용
    float speed = 1.0f;



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
        
        for(int ii = 0; ii < unitParentTr.childCount; ii++)
        {
            unitSpawnTr[ii] = unitParentTr?.GetChild(ii);
        }

        SceneType = Define.Scene.BattleStage_Field;
        Managers.Game.MonsterSpawnInit(monsterParentTr);
        Managers.Sound.Play("BGM/GameBGM", Define.Sound.BGM);
        TryGetComponent(out playable);

    }

    // Update is called once per frame
    void Update()
    {
        if (playable.state == PlayState.Playing)  //타임라인이 작동중이면 리턴
            return;

        if (Managers.UI.GetSceneUI<UI_GamePlay>() == null)
            return;



        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCoolTime(speed);

        Managers.Game.unitSummonDequeue(obj,unitSpawnTr);
        Managers.Game.CostIncreaseTime();
        Managers.Game.NormalMonsterSpawn(monSpawnType);

        if (Managers.Game.EliteMonsterCheck())
            monSpawnType = Define.MonsterSpawnType.Elite;

        if (monSpawnType == Define.MonsterSpawnType.Normal)
            monSpawnType = Managers.Game.MonsterWaveEvent(monSpawnType, 100);
        else if (monSpawnType == Define.MonsterSpawnType.Wave)
            monSpawnType = Managers.Game.MonsterWave(monSpawnType);
        //else if(monSpawnType == Define.MonsterSpawnType.Elite)
        //    monSpawnType = Managers.Game.EliteMonsterEventSpawn(monSpawnType);



        //Debug.Log(monSpawnType);

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
