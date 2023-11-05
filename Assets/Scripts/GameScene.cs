using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;




public class GameScene : BaseScene
{
    //게임씬에 들어왔을때 씬에서 필요한것들을 처음에 갱신해주는 역할

    private PlayableDirector playable;

    GameObject obj;           //유닛소환할때 큐에서 게임오브젝트를 받아줄 변수
    [SerializeField] Transform unitParentTr;
    Transform[] unitSpawnTr = new Transform[3];
    [SerializeField] Transform monsterParentTr;
    [SerializeField] TimelineAsset[] timeLines;

    private WarningNotice warningNotice;
    Define.MonsterSpawnType monSpawnType = Define.MonsterSpawnType.Normal;      //이벤트에서 관리한값을 받아서 적용
    float speed = 3.0f;
    int finalMonsterCount = 3;          //마지막이벤트에서 나올 엘리트몬스터의 숫자

    private readonly string warningWave = "게이트에서 몬스터가 대량으로 몰려옵니다 !..";
    private readonly string warningElite = "게이트에서 강력한 몬스터 개체 출현!";
    private readonly string warningFinal = "강력한 몬스터 여러 개체가 출현!!!";

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Managers.UI.CloseAllPopUpUI();
    }

    protected override void Init()
    {
        base.Init();
        
        for(int ii = 0; ii < unitParentTr.childCount; ii++)
        {
            unitSpawnTr[ii] = unitParentTr?.GetChild(ii);
        }
        

        SceneType = Define.Scene.BattleStage_Field;
        Managers.Game.EventInit();
        Managers.Game.MonsterSpawnInit(monsterParentTr);
        Managers.Game.SetMonSpawnType(Define.MonsterSpawnType.Normal);
        Managers.Game.SetStageStateType(Define.StageStageType.Playing);
        Managers.Sound.Play("BGM/GameBGM", Define.Sound.BGM);
        TryGetComponent(out playable);
        GameDirector(Define.GameStageDirector.Entrance);

    }

    // Update is called once per frame
    void Update()
    {
        if (playable.state == PlayState.Playing || Managers.Game.GameEndResult())  //타임라인이 작동중이면 리턴
            return;
        if (Managers.UI.GetSceneUI<UI_GamePlay>() == null)
            return;
        if (warningNotice == null)
            Managers.UI.GetSceneUI<UI_GamePlay>().TryGetComponent(out warningNotice);



        if (playable.playableAsset == timeLines[(int)Define.GameStageDirector.Victory])
            Managers.Game.ResultState(Define.StageStageType.Victory);
        //if (playable.playableAsset == timeLines[(int)Define.GameStageDirector.Defeat])
        //    Managers.Game.ResultState(Define.StageStageType.Defeat);






        if(!Managers.Game.GameEndResult())
        {
            Managers.Game.InGameTimer();

            //Debug.Log(Managers.Game.GetMonSpawnType());
            Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCoolTime(speed);
            Managers.UI.GetSceneUI<UI_GamePlay>().InGameTimer();

            Managers.Game.unitSummonDequeue(obj, unitSpawnTr);
            Managers.Game.CostIncreaseTime();
            Managers.Game.NormalMonsterSpawn(Managers.Game.GetMonSpawnType(), warningNotice.WarningObjisOn, warningElite);

            //Debug.Log(Managers.Game.GetMonSpawnType());
            if (Managers.Game.GetMonSpawnType() == Define.MonsterSpawnType.Normal)
            {
                //웨이브를 기다리는 타이머함수

                Managers.Game.MonsterWaveEvent(warningNotice.WarningObjisOn, warningWave);
                //경고창을 띄우는 함수를 보내서 Wave이벤트일때 사용
            }
            else if (Managers.Game.GetMonSpawnType() == Define.MonsterSpawnType.Wave)
            {
                //웨이브 지속시간 타이머함수
                Managers.Game.MonsterWave();
                //경고창을 띄우는 함수를 보내서 Wave이벤트일때 사용

            }
            else if (Managers.Game.GetMonSpawnType() == Define.MonsterSpawnType.Final)
            {
                Managers.Game.FinalMonsterWave(warningNotice.WarningObjisOn, warningFinal, finalMonsterCount);
            }
        }
        


    }


    public void UiOnOff(bool isOn)
    {
        if (Managers.UI.GetSceneUI<UI_GamePlay>() != null)
        {
            Managers.UI.GetSceneUI<UI_GamePlay>().gameObject.SetActive(isOn);
            Time.timeScale = 1.0f;
        }
    }


    public void GameDirector(Define.GameStageDirector stageDr)
    {
        //넣어준 타임라인연출을 시작함.
        playable.playableAsset = timeLines[(int)stageDr];
        playable.Play();
    }




    public override void Clear()
    {

    }



}
