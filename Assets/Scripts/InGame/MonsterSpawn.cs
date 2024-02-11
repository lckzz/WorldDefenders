using System;
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
    private int monsterCount = 0;           //몬스터 카운트

    public void Init()
    {
        isSpawn = false;
        monsterCount = 0;
        spawnTime = .0f;

    }
    public void MonsterSpawnInit(Transform parentTr)
    {
        monNameList.Clear();
        //인게임 들어올 때 스테이지에 맞는 몬스터들을 받는다.
        for (int ii = 0; ii < Managers.Game.MonsterTypeList.Count; ii++)
        {
            monNameList.Add(Enum.GetName(typeof(Define.MonsterType), Managers.Game.MonsterTypeList[ii]));
            spawnList.Add(Managers.Resource.Load<GameObject>($"Prefabs/Monster/{monNameList[ii]}"));
        }

        for (int i = 0; i < parentTr.childCount; i++)
             monsterSpawnPos[i] = parentTr.GetChild(i);

        Init();

    }


    public void MonsterSpawnTimer(Define.MonsterSpawnType spawnType,Action<string> action,string warningTxt)  //일반적인 몬스터 스폰
    {
        if(!isSpawn)
        {
            isSpawn = true;   //스폰타이머 시작
            if(spawnType == Define.MonsterSpawnType.Wave)
                spawnTime = UnityEngine.Random.Range(0.5f, 1.5f);
            else
                spawnTime = UnityEngine.Random.Range(5.0f, 7.0f);

            
        }
        else
        {
            //Debug.Log($"스폰타임{spawnTime}");
            spawnTime -= Time.deltaTime;
            if(spawnTime <= 0.0f)
            {
                spawnTime = 0.0f;
                int randidx = UnityEngine.Random.Range(0, 2);
                int randPosidx = UnityEngine.Random.Range(0, 3);
                GameObject go = Managers.Resource.Instantiate(spawnList[randidx], monsterSpawnPos[randPosidx].position);
                monsterCount++;
                if(monsterCount >= 30)
                {
                    monsterCount = 0;
                    Managers.Game.EliteMonsterEvent(action,warningTxt);
                    //Managers.Resource.Instantiate(spawnList[monNameList.Count - 1], monsterSpawnPos[randPosidx].position);
                }
                isSpawn = false;
            }
        }
    }


    public void EliteMonsterSpawn()
    {
        int randPosidx = UnityEngine.Random.Range(0, 3);
        Managers.Resource.Instantiate(spawnList[spawnList.Count - 1], monsterSpawnPos[randPosidx].position);

    }

}
