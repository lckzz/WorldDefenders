using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    //���� ���ӿ��� �����ִ� �÷���
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
            //unitnode.CoolCheck = true;  //�ش� ������ ��ȯ�� �Ǹ� ��Ÿ�� �߰� (true�� ���)
        }

    }



}
