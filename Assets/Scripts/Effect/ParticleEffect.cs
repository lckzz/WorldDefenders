using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    ParticleSystem particle;
    ParticleSystem particle2;

    float durationTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetChild(0).TryGetComponent(out particle);
        this.transform.GetChild(1).TryGetComponent(out particle2);
    }

    // Update is called once per frame
    void Update()
    {
        if(particle != null)
        {

            if (particle.time >= durationTime)
            {
                StartCoroutine(Util.UnitDieTime(gameObject,0.5f));

            }
        }
    }


}
