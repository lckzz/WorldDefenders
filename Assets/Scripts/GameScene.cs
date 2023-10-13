using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameScene : BaseScene
{
    //���Ӿ��� �������� ������ �ʿ��Ѱ͵��� ó���� �������ִ� ����
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
