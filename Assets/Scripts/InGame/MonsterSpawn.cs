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
    private int monsterCount = 0;           //���� ī��Ʈ

    public void Init()
    {
        isSpawn = false;
        monsterCount = 0;
    }
    public void MonsterSpawnInit(Transform parentTr)
    {
        monNameList.Clear();
        //�ΰ��� ���� �� ���������� �´� ���͵��� �޴´�.
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


    public void MonsterSpawnTimer(Define.MonsterSpawnType spawnType,Action<string> action,string warningTxt)  //�Ϲ����� ���� ����
    {
        if(!isSpawn)
        {
            isSpawn = true;   //����Ÿ�̸� ����
            if(spawnType == Define.MonsterSpawnType.Wave)
                spawnTime = UnityEngine.Random.Range(1.0f, 2.0f);
            else
                spawnTime = UnityEngine.Random.Range(6.0f, 9.0f);

            
        }
        else
        {
            //Debug.Log($"����Ÿ��{spawnTime}");
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
