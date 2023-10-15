using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn
{

    List<string> monNameList = new List<string>();
    List<GameObject> spawnList = new List<GameObject>();

    private Transform[] monsterSpawnPos = new Transform[3];
    private float spawnTime = .0f;
    private bool isSpawn = false;       


    public void MonsterSpawnInit(Transform parentTr)
    {
        monNameList.Clear();
        //인게임 들어올 때 스테이지에 맞는 몬스터들을 받는다.
        for (int ii = 0; ii < GlobalData.g_MonsterTypeList.Count; ii++)
        {
            monNameList.Add(System.Enum.GetName(typeof(Define.MonsterType), GlobalData.g_MonsterTypeList[ii]));
            spawnList.Add(Managers.Resource.Load<GameObject>($"Prefabs/Monster/{monNameList[ii]}"));
        }

        for (int i = 0; i < parentTr.childCount; i++)
        {
            monsterSpawnPos[i] = parentTr.GetChild(i);
        }
    }


    public void MonsterSpawnTimer(Define.MonsterSpawnType spawnType)  //일반적인 몬스터 스폰
    {
        if(!isSpawn)
        {
            isSpawn = true;   //스폰타이머 시작
            if(spawnType == Define.MonsterSpawnType.Wave)
                spawnTime = Random.Range(1.0f, 2.0f);
            else
                spawnTime = Random.Range(4.0f, 6.0f);

        }
        else
        {
            spawnTime -= Time.deltaTime;
            if(spawnTime <= 0.0f)
            {
                spawnTime = 0.0f;
                int randidx = Random.Range(0, 2);
                int randPosidx = Random.Range(0, 3);
                GameObject go = Managers.Resource.Instantiate(spawnList[randidx], monsterSpawnPos[randPosidx].position);
                Debug.Log(go);
                isSpawn = false;
            }
        }
    }


    public void EliteMonsterSpawn()
    {
        int randPosidx = Random.Range(0, 3);
        GameObject go = Managers.Resource.Instantiate(spawnList[spawnList.Count - 1], monsterSpawnPos[randPosidx].position);

    }

}
