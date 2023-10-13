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


        //if (UI_GamePlay.gameQueue.Count > 0)
        //{
        //    Managers.UI.GetSceneUI<UI_GamePlay>().Summon(obj, spawnPos);
        //    //go.TryGetComponent(out unitnode);
        //    //unitnode.CoolCheck = true;  //�ش� ������ ��ȯ�� �Ǹ� ��Ÿ�� �߰� (true�� ���)
        //}

        Managers.Game.unitSummonDequeue(obj,unitSpawnTr);
        Managers.Game.CostIncreaseTime();
        Managers.Game.NormalMonsterSpawn();

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
