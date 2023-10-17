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

    private WarningNotice warningNotice;
    Define.MonsterSpawnType monSpawnType = Define.MonsterSpawnType.Normal;      //�̺�Ʈ���� �����Ѱ��� �޾Ƽ� ����
    float speed = 1.0f;
    int finalMonsterCount = 3;          //�������̺�Ʈ���� ���� ����Ʈ������ ����

    private readonly string warningWave = "����Ʈ���� ���Ͱ� �뷮���� �����ɴϴ�!..";
    private readonly string warningElite = "����Ʈ���� ������ ���� ��ü ����!";
    private readonly string warningFinal = "������ ���� ���� ��ü�� ����!!!";

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
        Managers.Game.SetStageStateType(Define.StageStageType.Playing);
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
        if (Managers.Game.GameEndResult())
            return;


        if(warningNotice == null)
            Managers.UI.GetSceneUI<UI_GamePlay>().TryGetComponent(out warningNotice);


        //Debug.Log(Managers.Game.GetMonSpawnType());
        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCoolTime(speed);

        Managers.Game.unitSummonDequeue(obj,unitSpawnTr);
        Managers.Game.CostIncreaseTime();
        Managers.Game.NormalMonsterSpawn(Managers.Game.GetMonSpawnType(), warningNotice.WarningObjisOn, warningElite);

        //Debug.Log(Managers.Game.GetMonSpawnType());
        if (Managers.Game.GetMonSpawnType() == Define.MonsterSpawnType.Normal)
        {
            //���̺긦 ��ٸ��� Ÿ�̸��Լ�

            Managers.Game.MonsterWaveEvent(warningNotice.WarningObjisOn, warningWave);
            //���â�� ���� �Լ��� ������ Wave�̺�Ʈ�϶� ���
        }
        else if (Managers.Game.GetMonSpawnType() == Define.MonsterSpawnType.Wave)
        {
            //���̺� ���ӽð� Ÿ�̸��Լ�
            Managers.Game.MonsterWave();
            //���â�� ���� �Լ��� ������ Wave�̺�Ʈ�϶� ���

        }
        else if(Managers.Game.GetMonSpawnType() == Define.MonsterSpawnType.Final)
        {
            Managers.Game.FinalMonsterWave(warningNotice.WarningObjisOn,warningFinal,finalMonsterCount);
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
