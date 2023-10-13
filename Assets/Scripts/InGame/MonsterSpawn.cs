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


    public void NormalMonsterSpawn()  //�Ϲ����� ���� ����
    {
        if(!isSpawn)
        {
            isSpawn = true;   //����Ÿ�̸� ����
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
    
}
