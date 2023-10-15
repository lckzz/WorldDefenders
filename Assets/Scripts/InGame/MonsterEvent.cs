using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEvent
{
    //게임내의 몬스터 스폰 타입을 이벤트에 맞게 변경해주는 클래스
    private bool monsterWaveisOn = false;
    private bool eliteMonsterisOn = false;
  

    private float monsterWaveTime = 10.0f;

    private float monsterWaveDuration = 10.0f; //지속 시간

    private int eliteMonsterCount = 2;
    private Define.MonsterSpawnType monSpawnType = Define.MonsterSpawnType.Normal;

    public bool MonsterWaveisOn { get { return monsterWaveisOn; } set { monsterWaveisOn = value; } }
    public bool EliteMonsterisOn { get { return eliteMonsterisOn; } set { eliteMonsterisOn = value; } }


    public void Init()
    {
        monsterWaveTime = 10.0f;
        monsterWaveDuration = 10.0f;
        monsterWaveisOn = false;
        eliteMonsterisOn = false;
}


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

    public bool EliteMonsterCheck()
    {
        if (eliteMonsterisOn)
            return true;
        else
            return false;
    }

    public void EliteMonsterEvent()
    {
        if(eliteMonsterisOn == false)
        {
            eliteMonsterisOn = true;
        }
    }

    public Define.MonsterSpawnType EliteMonsterEventSpawn(Define.MonsterSpawnType spawnType)
    {
        Managers.Game.EliteMonsterSpawn();
  
        spawnType = Define.MonsterSpawnType.Normal;

        return spawnType;
    }
}
