using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    //float speed = 1.0f;
    //float curCost = .0f;
    //float maxCost = 500.0f;
    //float costCoolTime = 1.0f;
    [SerializeField] Transform[] spawnPos;
    GameObject obj;
    UnitNode unitnode;
    float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Managers.UI.ShowSceneUI<UI_GamePlay>();

        Debug.Log(Managers.UI.GetSceneUI<UI_GamePlay>());

    }

    // Update is called once per frame
    void Update()
    {

        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCoolTime(speed);
        Managers.Game.CostCoolTimer(2.5f,30.0f);

        if(UI_GamePlay.gameQueue.Count > 0)
        {
            Managers.UI.GetSceneUI<UI_GamePlay>().Summon(obj, spawnPos);
            //go.TryGetComponent(out unitnode);
            //unitnode.CoolCheck = true;  //해당 유닛이 소환이 되면 쿨타임 추가 (true면 쿨온)
        }

    }



}
