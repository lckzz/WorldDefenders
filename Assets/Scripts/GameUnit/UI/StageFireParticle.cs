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


        if (particleDict.ContainsKey(particleFire))  //해당 키값이 있다면
        {
            if (particleDict[particleFire]())
                this.gameObject.SetActive(false);       //키값을 통해서 클리어판정이 떳으면 해당 파티클끄기

        }


    }






}
