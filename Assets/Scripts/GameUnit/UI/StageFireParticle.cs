using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFireParticle : MonoBehaviour
{
    public enum ParticleFire
    {
        West,
        East,
        South,
        None
    }

    [SerializeField] private ParticleFire particleFire = ParticleFire.None;
    private Dictionary<ParticleFire, Func<bool>> particleDict = new Dictionary<ParticleFire, Func<bool>>
    {
        {ParticleFire.West, () => Managers.Game.WestStageClear },
        {ParticleFire.East, () => Managers.Game.EastStageClear },
        {ParticleFire.South, () => Managers.Game.SouthStageClear },


    };


    // Start is called before the first frame update
    void Start()
    {
        if (particleDict.ContainsKey(particleFire))  //해당 키값이 있다면
        {
            if (particleDict[particleFire]())
                this.gameObject.SetActive(false);       //키값을 통해서 클리어판정이 떳으면 해당 파티클끄기

        }


    }






}
