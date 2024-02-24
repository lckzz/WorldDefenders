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



    private float monsterWaveTime = 30.0f;

    private float monsterWaveDuration = 15.0f; //지속 시간

    private int finalEliteMonsterCount = 0;
    private bool finalOneWarningUI = false;
    private float finalEliteMonsterTime = 1.0f;

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
                case Define.MonsterSpawnType.Normal:        //일반적인 몬스터 스폰
                    monsterSpawnNormalisOn = true;
                    monsterWaveisOn = false;
                    eliteMonsterisOn = false;
                    finalMonsterisOn = false;
                    break;
                case Define.MonsterSpawnType.Wave:         //몬스터가 몰려오는 웨이브
                    monsterSpawnNormalisOn = false;
                    monsterWaveisOn = true;
                    eliteMonsterisOn = false;
                    finalMonsterisOn = false;
                    break;
                case Define.MonsterSpawnType.Final:        //마지막에 돌입하면 보스웨이브만오고 나머지 웨이브는 오지않는다.
                    monsterSpawnNormalisOn = false;
                    monsterWaveisOn = false;
                    eliteMonsterisOn = false;
                    finalMonsterisOn = true;
                    break;
            }
        } 
    }

    public void EventInit()             //이벤트의 변수들을 초기화해준다.
    {
        monSpawnType = Define.MonsterSpawnType.Normal;
        monsterWaveTime = 30.0f;
        monsterWaveDuration = 15.0f;
        finalEliteMonsterTime = 1.0f;
        monsterWaveisOn = false;
        eliteMonsterisOn = false;
        finalOneWarningUI = false;
        finalMonsterisOn = false;
        finalEliteMonsterCount = 0;
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


    public void MonsterWaveEvent(Action<string> action,string warningTxt)
    {
        //일반 이벤트중일때

        //Debug.Log(monsterWaveTime);
        monsterWaveTime -= Time.deltaTime;      //몬스터 웨이브 시간이 흐른다.
        if(monsterWaveTime <= 0.0f)         //몬스터 웨이브시간이 되면
        {
            monsterWaveTime = 0.0f;     
            monsterWaveisOn = true;         //몬스터웨이브를 켜준다.
            monsterWaveDuration = 15.0f;      //해당 시간동안 웨이브
            MonSpawnType = Define.MonsterSpawnType.Wave;        //타입을 웨이브로 바꿔준다.
            action(warningTxt);  //받아온 액션함수를 사용(경고 텍스트가 나타난다)
            //Debug.Log("여기 웨이브");
        }



    }


    public void MonsterWave()             //일반적인 이벤트
    {
        //웨이브 이벤트중이라면
        monsterWaveDuration -= Time.deltaTime;          //웨이브 지속시간
        if(monsterWaveDuration <= 0.0f)         //지속시간이 다되면
        {

            monsterWaveisOn = false;            //웨이브를 꺼준다.
            monsterWaveDuration = 0.0f;
            monsterWaveTime = 30.0f;            //웨이브시간을 다시 설정해준다.
            MonSpawnType = Define.MonsterSpawnType.Normal;      //일반상태로 변환한다.
            

        }

    }


    public void EliteMonsterEvent(Action<string> action, string warningTxt)         //엘리트몬스터 등장 이벤트
    {
        eliteMonsterisOn = true;            //엘리트몬스터 등장을 켜준다.
        action(warningTxt);                 //경고텍스트를 보여준다.

        Managers.Game.EliteMonsterSpawn();      //엘리트몬스터를 소환한다.

    }

    public void FinalMonsterWave(Action<string> action, string warningTxt, int eliteMonsterCount)  //일반 스테이지에서 마지막 웨이브
    {
        if (finalEliteMonsterCount >= eliteMonsterCount)            //마지막몬스터갯수가 설정된값보다 많아지면 리턴
            return;

        if (finalOneWarningUI == false)
        {
            finalOneWarningUI = true;
            action(warningTxt);       //경고창 띄우기


        }

        if (Managers.Game.CurStageType != Define.Stage.Boss)         //보스 스테이지가 아닐때는 엘리트몹 3마리 소환함
        {

            finalEliteMonsterTime -= Time.deltaTime;

            if (finalEliteMonsterTime <= 0.0f)
            {
                Managers.Game.EliteMonsterSpawn();  //몬스터를 소환하고
                finalEliteMonsterCount++;
                finalEliteMonsterTime = 1.5f;
            }
        }

        else           //보스 스테이지일떄는 보스한마리만 소환함
        {

            Managers.Game.BossMonsterSpawn();  //해당 보스몬스터를 소환함
            finalEliteMonsterCount++;

        }





    }
}
