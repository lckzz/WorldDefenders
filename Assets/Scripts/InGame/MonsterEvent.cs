using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEvent
{
    //게임내의 몬스터 스폰 타입을 이벤트에 맞게 변경해주는 클래스
    private bool monsterSpawnNormalisOn = false;
    private bool monsterWaveisOn = false;
    private bool eliteMonsterisOn = false;
    private bool finalMonsterisOn = false;



    private float monsterWaveTime = 10.0f;

    private float monsterWaveDuration = 10.0f; //지속 시간

    private int finalEliteMonsterCount = 0;
    private bool finalOneWarningUI = false;
    private float finalEliteMonsterTime = 2.5f;

    private int eliteMonsterCount = 2;

    private Define.MonsterSpawnType monSpawnType = Define.MonsterSpawnType.Normal;

    public bool MonsterWaveisOn { get { return monsterWaveisOn; } set { monsterWaveisOn = value; } }
    public bool EliteMonsterisOn { get { return eliteMonsterisOn; } set { eliteMonsterisOn = value; } }
    public bool MonsterSpawnNormalisOn { get { return monsterSpawnNormalisOn; } set { monsterSpawnNormalisOn = value; } }
    public bool FinalMonsterisOn { get { return finalMonsterisOn; } set { finalMonsterisOn = value; } }



    public Define.MonsterSpawnType MonSpawnType 
    {   get { return monSpawnType; } 
        set
        {
            monSpawnType = value;
            switch (monSpawnType)
            {
                case Define.MonsterSpawnType.Normal:
                    monsterSpawnNormalisOn = true;
                    monsterWaveisOn = false;
                    eliteMonsterisOn = false;
                    finalMonsterisOn = false;
                    break;
                case Define.MonsterSpawnType.Wave:
                    monsterSpawnNormalisOn = false;
                    monsterWaveisOn = true;
                    eliteMonsterisOn = false;
                    finalMonsterisOn = false;
                    break;
                case Define.MonsterSpawnType.Final:
                    monsterSpawnNormalisOn = false;
                    monsterWaveisOn = false;
                    eliteMonsterisOn = false;
                    finalMonsterisOn = true;
                    break;
            }
        } 
    }

    public void EventInit()
    {
        monSpawnType = Define.MonsterSpawnType.Normal;
        monsterWaveTime = 10.0f;
        monsterWaveDuration = 10.0f;
        finalEliteMonsterTime = 2.5f;
        monsterWaveisOn = false;
        eliteMonsterisOn = false;
        finalOneWarningUI = false;
    }



    public bool MonsterNormalCheck()
    {
        if (monsterSpawnNormalisOn)
            return true;

        else
            return false;
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
    public bool FinalMonsterCheck()
    {
        if (finalMonsterisOn)
            return true;
        else
            return false;
    }


    public void MonsterWaveEvent(Action action)
    {
        //일반 이벤트중일때

        monsterWaveTime -= Time.deltaTime;
        if(monsterWaveTime <= 0.0f)
        {
            monsterWaveTime = 0.0f;
            monsterWaveisOn = true;
            monsterWaveDuration = 10.0f;
            MonSpawnType = Define.MonsterSpawnType.Wave;
            action();  //받아온 액션함수를 사용
            //Debug.Log("여기 웨이브");
        }



    }


    public void MonsterWave()
    {
        //웨이브 이벤트중이라면
        monsterWaveDuration -= Time.deltaTime;
        if(monsterWaveDuration <= 0.0f)
        {

            monsterWaveisOn = false;
            monsterWaveDuration = 0.0f;
            monsterWaveTime = 20.0f;
            MonSpawnType = Define.MonsterSpawnType.Normal;
            

        }

    }


    public void EliteMonsterEvent(Action action)
    {
        eliteMonsterisOn = true;
        action();
        Managers.Game.EliteMonsterSpawn();

    }

    public void FinalMonsterWave(Action action,int eliteMonsterCount)
    {
        if (finalEliteMonsterCount > eliteMonsterCount)
            return;

        if(finalOneWarningUI == false)
        {
            finalOneWarningUI = true;
            action();       //경고창 띄우기

        }
        finalEliteMonsterTime -= Time.deltaTime;

        if(finalEliteMonsterTime <= 0.0f)
        {
            Managers.Game.EliteMonsterSpawn();  //몬스터를 소환하고
            finalEliteMonsterCount++;
            finalEliteMonsterTime = 1.5f;
        }


    }
}
