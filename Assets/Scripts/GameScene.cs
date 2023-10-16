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

    private WarningNotice warningNotice;
    Define.MonsterSpawnType monSpawnType = Define.MonsterSpawnType.Normal;      //이벤트에서 관리한값을 받아서 적용
    float speed = 1.0f;
    int finalMonsterCount = 3;          //마지막이벤트에서 나올 엘리트몬스터의 숫자



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
        Managers.Game.SetMonSpawnType(Define.MonsterSpawnType.Normal);
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
        if(warningNotice == null)
            Managers.UI.GetSceneUI<UI_GamePlay>().TryGetComponent(out warningNotice);


        //Debug.Log(Managers.Game.GetMonSpawnType());
        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCoolTime(speed);

        Managers.Game.unitSummonDequeue(obj,unitSpawnTr);
        Managers.Game.CostIncreaseTime();
        Managers.Game.NormalMonsterSpawn(Managers.Game.GetMonSpawnType(), warningNotice.WarningObjisOn);

        //Debug.Log(Managers.Game.GetMonSpawnType());
        if (Managers.Game.GetMonSpawnType() == Define.MonsterSpawnType.Normal)
        {
            //웨이브를 기다리는 타이머함수

            Managers.Game.MonsterWaveEvent(warningNotice.WarningObjisOn);
            //경고창을 띄우는 함수를 보내서 Wave이벤트일때 사용
        }
        else if (Managers.Game.GetMonSpawnType() == Define.MonsterSpawnType.Wave)
        {
            //웨이브 지속시간 타이머함수
            Managers.Game.MonsterWave();
            //경고창을 띄우는 함수를 보내서 Wave이벤트일때 사용

        }
        else if(Managers.Game.GetMonSpawnType() == Define.MonsterSpawnType.Final)
        {
            Managers.Game.FinalMonsterWave(warningNotice.WarningObjisOn, finalMonsterCount);
        }
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
