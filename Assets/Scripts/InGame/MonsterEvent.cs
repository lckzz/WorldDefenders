using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEvent
{
    //���ӳ��� ���� ���� Ÿ���� �̺�Ʈ�� �°� �������ִ� Ŭ����
    private bool monsterSpawnNormalisOn = false;
    private bool monsterWaveisOn = false;
    private bool eliteMonsterisOn = false;
    private bool finalMonsterisOn = false;



    private float monsterWaveTime = 30.0f;

    private float monsterWaveDuration = 15.0f; //���� �ð�

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
                case Define.MonsterSpawnType.Normal:        //�Ϲ����� ���� ����
                    monsterSpawnNormalisOn = true;
                    monsterWaveisOn = false;
                    eliteMonsterisOn = false;
                    finalMonsterisOn = false;
                    break;
                case Define.MonsterSpawnType.Wave:         //���Ͱ� �������� ���̺�
                    monsterSpawnNormalisOn = false;
                    monsterWaveisOn = true;
                    eliteMonsterisOn = false;
                    finalMonsterisOn = false;
                    break;
                case Define.MonsterSpawnType.Final:        //�������� �����ϸ� �������̺길���� ������ ���̺�� �����ʴ´�.
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
        //�Ϲ� �̺�Ʈ���϶�

        //Debug.Log(monsterWaveTime);
        monsterWaveTime -= Time.deltaTime;
        if(monsterWaveTime <= 0.0f)
        {
            monsterWaveTime = 0.0f;
            monsterWaveisOn = true;
            monsterWaveDuration = 15.0f;
            MonSpawnType = Define.MonsterSpawnType.Wave;
            action(warningTxt);  //�޾ƿ� �׼��Լ��� ���
            //Debug.Log("���� ���̺�");
        }



    }


    public void MonsterWave()             //�Ϲ����� �̺�Ʈ
    {
        //���̺� �̺�Ʈ���̶��
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

    public void FinalMonsterWave(Action<string> action, string warningTxt, int eliteMonsterCount)  //�Ϲ� ������������ ������ ���̺�
    {
        if (finalEliteMonsterCount >= eliteMonsterCount)
            return;

        if(finalOneWarningUI == false)
        {
            finalOneWarningUI = true;
            action(warningTxt);       //���â ����


        }

        if(Managers.Game.CurStageType != Define.Stage.Boss)         //���� ���������� �ƴҶ��� ����Ʈ�� 3���� ��ȯ��
        {

            finalEliteMonsterTime -= Time.deltaTime;

            if (finalEliteMonsterTime <= 0.0f)
            {
                Managers.Game.EliteMonsterSpawn();  //���͸� ��ȯ�ϰ�
                finalEliteMonsterCount++;
                finalEliteMonsterTime = 1.5f;
            }
        }

        else           //���� ���������ϋ��� �����Ѹ����� ��ȯ��
        {

            BossFinalWave();
            finalEliteMonsterCount++;

        }



    }


    private void BossFinalWave()
    {
        Managers.Game.BossMonsterSpawn();  //�ش� �������͸� ��ȯ��

    }


}
