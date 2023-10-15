using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameScene : BaseScene
{
    //���Ӿ��� �������� ������ �ʿ��Ѱ͵��� ó���� �������ִ� ����

    private PlayableDirector playable;

    GameObject obj;           //���ּ�ȯ�Ҷ� ť���� ���ӿ�����Ʈ�� �޾��� ����
    [SerializeField] Transform unitParentTr;
    Transform[] unitSpawnTr = new Transform[3];
    [SerializeField] Transform monsterParentTr;
    Define.MonsterSpawnType monSpawnType = Define.MonsterSpawnType.Normal;      //�̺�Ʈ���� �����Ѱ��� �޾Ƽ� ����
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
        if (playable.state == PlayState.Playing)  //Ÿ�Ӷ����� �۵����̸� ����
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
