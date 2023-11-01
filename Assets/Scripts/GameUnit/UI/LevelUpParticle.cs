using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelUpParticle : MonoBehaviour
{
    private ParticleSystem particle;
    private float duration = 2.5f;
    private float time = 0.0f;

    [SerializeField] private ParticleSystem textParticle;
    private Vector3 textParticlePos = new Vector3(0.0f,-25.0f,0.0f);

    Coroutine coroutine = null;

    private void OnEnable()
    {
        if (particle == null)
            TryGetComponent(out particle);

        if(coroutine != null)
            StopCoroutine(coroutine);
        

        time = 0.0f;
        textParticle.gameObject.transform.localPosition = textParticlePos;
        particle.Play();
        coroutine = StartCoroutine(DurationParticleTime());
    }



    IEnumerator DurationParticleTime()
    {
        textParticle?.transform.DOKill();
        textParticle?.transform.DOLocalMoveY(110.0f, 1.0f).SetEase(Ease.InOutBack);

        while (true)
        {

            if (time >= duration)
            {
                gameObject.SetActive(false);
                yield break;
            }

            else
                time += Time.deltaTime;


            yield return null;
        }
    }

    public void DoKill()
    {
        DOTween.KillAll();     //연결을 끊어준다.
    }

}
