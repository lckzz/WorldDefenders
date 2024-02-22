using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn
{

    private List<string> monNameList = new List<string>();
    private List<GameObject> spawnList = new List<GameObject>();
    private GameObject eliteSpawn;
    private GameObject bossSpawn;


    private Transform[] monsterSpawnPos = new Transform[3];
    private float spawnTime = .0f;
    private bool isSpawn = false;
    private int monsterCount = 0;           //���� ī��Ʈ

    public void Init()
    {
        isSpawn = false;
        monsterCount = 0;
        spawnTime = .0f;

    }
    public void MonsterSpawnInit(Transform parentTr)
    {
        monNameList.Clear();
        spawnList.Clear();
        eliteSpawn = null;
        bossSpawn = null;

        //�ΰ��� ���� �� ���������� �´� ���͵��� �޴´�.
        for (int ii = 0; ii < Managers.Game.MonsterTypeList.Count; ii++)        //���������� ����Ÿ�Ը���Ʈ�� ���Ƽ�
        {
            monNameList.Add(Enum.GetName(typeof(Define.MonsterType), Managers.Game.MonsterTypeList[ii]));
            spawnList.Add(Managers.Resource.Load<GameObject>($"Prefabs/Monster/{monNameList[ii]}"));            //��������Ʈ�� ���͵��� �־��ش�.
        }
         

        string eliteStr = Enum.GetName(typeof(Define.MonsterType), Managers.Game.OneChapterStageInfoList[(int)Managers.Game.CurStageType].StageData.eliteMonster);
        eliteSpawn = Managers.Resource.Load<GameObject>($"Prefabs/Monster/{eliteStr}");         //���������� ����Ʈ���͵� �־��ش�.
        
        if(Managers.Game.OneChapterStageInfoList[(int)Managers.Game.CurStageType].StageData.bossMonster >= (int)Define.MonsterType.SkeletonKing)
        {
            string bossStr = Enum.GetName(typeof(Define.MonsterType), Managers.Game.OneChapterStageInfoList[(int)Managers.Game.CurStageType].StageData.bossMonster);
            bossSpawn = Managers.Resource.Load<GameObject>($"Prefabs/Monster/{bossStr}");   //��������������� ������ �־��ش�.
        }


        for (int i = 0; i < parentTr.childCount; i++)
             monsterSpawnPos[i] = parentTr.GetChild(i);     //���� ������Ҹ� �����Ѵ�.

        Init();  //���� �ʱ�ȭ

    }


    public void MonsterSpawnTimer(Define.MonsterSpawnType spawnType,Action<string> action,string warningTxt)  //�Ϲ����� ���� ����
    {
        if(!isSpawn)
        {
            isSpawn = true;   //����Ÿ�̸� ����
            if(spawnType == Define.MonsterSpawnType.Wave)       //���̺��� ����Ÿ���� �������ְ�
                spawnTime = UnityEngine.Random.Range(0.5f, 1.5f);
            else
                spawnTime = UnityEngine.Random.Range(5.0f, 7.0f);       //�ƴ϶�� �γ��ϰ� �����ȴ�.

            
        }
        else
        {
            //Debug.Log($"����Ÿ��{spawnTime}");
            spawnTime -= Time.deltaTime;        //�����ð��� �帣��
            if(spawnTime <= 0.0f)
            {
                spawnTime = 0.0f;       //�����ҽð��� �Ǹ�
                int randidx = UnityEngine.Random.Range(0, spawnList.Count);
                int randPosidx = UnityEngine.Random.Range(0, 3);        //������ ��Ҹ� �������� ���� �����Ѵ�.
                GameObject go = Managers.Resource.Instantiate(spawnList[randidx], monsterSpawnPos[randPosidx].position);
                monsterCount++;     //����ī��Ʈ�� �þ��
                if(monsterCount >= 30)              //��ȯ�� ���Ͱ� 30������ ������ ����Ʈ���� ����
                {
                    monsterCount = 0;
                    Managers.Game.EliteMonsterEvent(action,warningTxt);
                    //Managers.Resource.Instantiate(spawnList[monNameList.Count - 1], monsterSpawnPos[randPosidx].position);
                }
                isSpawn = false;
            }
        }
    }


    public void EliteMonsterSpawn()     //����Ʈ���� ����
    {
        int randPosidx = UnityEngine.Random.Range(0, 3);
        Managers.Resource.Instantiate(eliteSpawn, monsterSpawnPos[randPosidx].position);


    }

    public void BossMonsterSpawn()      //�������� ����
    {
        if (Managers.Game.CurStageType != Define.Stage.Boss)
            return;

        //int randPosidx = UnityEngine.Random.Range(0, 3);
        Managers.Resource.Instantiate(bossSpawn, monsterSpawnPos[1].position);      //������ ������ �߾ӿ��� ����


    }

}
