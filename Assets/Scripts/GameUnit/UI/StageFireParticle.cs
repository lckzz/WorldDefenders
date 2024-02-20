using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFireParticle : MonoBehaviour
{
    public enum ParticleFire
    {
        West,
        South,
        East,
        Boss,
        None
    }

    private Define.Stage stage;
    public ParticleFire particleFire = ParticleFire.None;
    private Dictionary<ParticleFire, Func<bool>> particleDict;


    // Start is called before the first frame update
    void Start()
    {
        

        particleDict = new Dictionary<ParticleFire, Func<bool>>
        {
            {ParticleFire.West, () => Managers.Game.OneChapterStageInfoList[(int)particleFire].clear },
            {ParticleFire.South, () => Managers.Game.OneChapterStageInfoList[(int)particleFire].clear },
            {ParticleFire.East, () => Managers.Game.OneChapterStageInfoList[(int)particleFire].clear },
            {ParticleFire.Boss, () => Managers.Game.OneChapterStageInfoList[(int)particleFire].clear },

        };


        if (particleDict.ContainsKey(particleFire))  //�ش� Ű���� �ִٸ�
        {
            if (particleDict[particleFire]())
                this.gameObject.SetActive(false);       //Ű���� ���ؼ� Ŭ���������� ������ �ش� ��ƼŬ����

        }


    }






}
