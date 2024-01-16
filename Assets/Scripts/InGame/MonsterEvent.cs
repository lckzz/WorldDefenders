using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEvent
{
    //惟績鎧税 佼什斗 什肉 展脊聖 戚坤闘拭 限惟 痕井背爽澗 適掘什
    private bool monsterSpawnNormalisOn = false;
    private bool monsterWaveisOn = false;
    private bool eliteMonsterisOn = false;
    private bool finalMonsterisOn = false;



    private float monsterWaveTime = 30.0f;

    private float monsterWaveDuration = 15.0f; //走紗 獣娃

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
        monsterWaveTime = 30.0f;
        monsterWaveDuration = 15.0f;
        finalEliteMonsterTime = 1.0f;
        monsterWaveisOn = false;
        eliteMonsterisOn = false;
        finalOneWarningUI = false;
        finalMonsterisOn = false;
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
        //析鋼 戚坤闘掻析凶

        //Debug.Log(monsterWaveTime);
        monsterWaveTime -= Time.deltaTime;
        if(monsterWaveTime <= 0.0f)
        {
            monsterWaveTime = 0.0f;
            monsterWaveisOn = true;
            monsterWaveDuration = 15.0f;
            MonSpawnType = Define.MonsterSpawnType.Wave;
            action(warningTxt);  //閤焼紳 衝芝敗呪研 紫遂
            //Debug.Log("食奄 裾戚崎");
        }



    }


    public void MonsterWave()
    {
        //裾戚崎 戚坤闘掻戚虞檎
        monsterWaveDuration -= Time.deltaTime;
        if(monsterWaveDuration <= 0.0f)
        {

            monsterWaveisOn = false;
            monsterWaveDuration = 0.0f;
            monsterWaveTime = 30.0f;
            MonSpawnType = Define.MonsterSpawnType.Normal;
            

        }

    }


    public void EliteMonsterEvent(Action<string> action, string warningTxt)
    {
        eliteMonsterisOn = true;
        action(warningTxt);
        Managers.Game.EliteMonsterSpawn();

    }

    public void FinalMonsterWave(Action<string> action, string warningTxt, int eliteMonsterCount)
    {
        if (finalEliteMonsterCount >= eliteMonsterCount)
            return;

        if(finalOneWarningUI == false)
        {
            finalOneWarningUI = true;
            action(warningTxt);       //井壱但 句酔奄
            Debug.Log("督戚確 戚坤動いいいいいいいいいいいいいいいい");


        }
        finalEliteMonsterTime -= Time.deltaTime;

        if(finalEliteMonsterTime <= 0.0f)
        {
            Managers.Game.EliteMonsterSpawn();  //佼什斗研 社発馬壱
            finalEliteMonsterCount++;
            finalEliteMonsterTime = 1.5f;
        }


    }
}
