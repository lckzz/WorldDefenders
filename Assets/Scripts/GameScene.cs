using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    //현재 게임에서 보여주는 플레이
    //float speed = 1.0f;
    //float curCost = .0f;
    //float maxCost = 500.0f;
    //float costCoolTime = 1.0f;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] float costCoolTime = 4.5f;
    GameObject obj;
    UnitNode unitnode;
    float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Managers.UI.ShowSceneUI<UI_GamePlay>();

        Debug.Log(Managers.UI.GetSceneUI<UI_GamePlay>());

        Dictionary<int,TowerStat> dict = Managers.Data.towerDict;

    }

    // Update is called once per frame
    void Update()
    {

        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCoolTime(speed);
        Managers.Game.CostCoolTimer(costCoolTime, 30.0f);

        if (UI_GamePlay.gameQueue.Count > 0)
        {
            Managers.UI.GetSceneUI<UI_GamePlay>().Summon(obj, spawnPos);
            //go.TryGetComponent(out unitnode);
            //unitnode.CoolCheck = true;  //해당 유닛이 소환이 되면 쿨타임 추가 (true면 쿨온)
        }

    }



}
