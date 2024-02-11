using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealParticle : MonoBehaviour
{
    private ParticleSystem healParticle1;
    private ParticleSystem healParticle2;

    private float timer = 0.0f;
    private float maxTime = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        if (healParticle1 == null)
            this.gameObject.transform.GetChild(0).TryGetComponent(out healParticle1);
        if (healParticle2 == null)
            this.gameObject.transform.GetChild(1).TryGetComponent(out healParticle2);


    }

    private void OnEnable()
    {
        if(healParticle1 == null)
            this.gameObject.transform.GetChild(0).TryGetComponent(out healParticle1);
        if (healParticle2 == null)
            this.gameObject.transform.GetChild(1).TryGetComponent(out healParticle2);



        healParticle1.Play();
        healParticle2.Play();

        StartCoroutine(ParticleTimerCo());

    }


    public void ParticleTimer()
    {
        StartCoroutine(ParticleTimerCo());
    }

    private IEnumerator ParticleTimerCo()
    {
        timer = 0.0f;

        while(true)
        {
            //Debug.Log(healParticle1.time);

            if (timer < maxTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //시간이 다지나면 파티클오브젝트가 꺼짐
                timer = 0.0f;
                gameObject.SetActive(false);
            }
            yield return null;
        }
    }


}
