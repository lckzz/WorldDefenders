using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEvent
{
    //���ӳ��� ���� ���� Ÿ���� �̺�Ʈ�� �°� �������ִ� Ŭ����
    private bool monsterWaveisOn = false;
    private bool eliteMonsterisOn = false;

    private float monsterWaveTime = 10.0f;

    private float monsterWaveDuration = 10.0f; //���� �ð�


    public bool MonsterWaveisOn { get { return monsterWaveisOn; } set { monsterWaveisOn = value; } }
    public bool EliteMonsterisOn { get { return eliteMonsterisOn; } set { eliteMonsterisOn = value; } }


    public Define.MonsterSpawnType MonsterWaveEvent(Define.MonsterSpawnType spawnType,int gateHp)
    {
        if (spawnType == Define.MonsterSpawnType.Wave)
            return spawnType;

        monsterWaveTime -= Time.deltaTime;
        if(monsterWaveTime <= 0.0f)
        {
            monsterWaveTime = 0.0f;
            monsterWaveisOn = true;
            monsterWaveDuration = 10.0f;
            spawnType = Define.MonsterSpawnType.Wave;
        }


        return spawnType;

    }


    public Define.MonsterSpawnType MonsterWave(Define.MonsterSpawnType spawnType)
    {

        monsterWaveDuration -= Time.deltaTime;
        if(monsterWaveDuration <= 0.0f)
        {

            monsterWaveisOn = false;
            monsterWaveDuration = 0.0f;
            monsterWaveTime = 20.0f;
            spawnType = Define.MonsterSpawnType.Normal;

        }


        return spawnType;

    }

    public bool MonsterWaveCheck()
    {
        if (monsterWaveisOn)
            return true;

        else
            return false;
    }
}
