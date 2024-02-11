using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUIBase : MonoBehaviour,IHpObserver
{
    [SerializeField]
    protected Image hpbar;
    [SerializeField]
    protected Image hpbarCover;
    protected float minHp = .0f;
    [SerializeField] protected float hpSpeed = 1.5f;
    protected SpriteRenderer spr;
    protected Color color = new Color(0, 0, 0);

    protected float destoryTimer = 1.5f;
    protected bool startImgFadeOut = false;
    protected float hp;

    private Coroutine updateHpCo = null;
    protected IHpSubject hpSubject;

    private void OnEnable()
    {
        destoryTimer = 1.5f;
        startImgFadeOut = false;
        hpbarCover.gameObject.SetActive(true);
        hpbar.fillAmount = 1.0f;
    }

    protected virtual void Start()
    {
        TryGetComponent(out hpSubject);
        hpSubject?.AddHpObserver(this);     //ó�������ϸ� �������ν� ����
    }

    protected T ComponentInit<T>(out T compo) where T : Component
    {
        if(TryGetComponent<T>(out compo))
            return compo;
        else
            return null;
    }

    //public virtual void UpdateHp(float hpspeed) { }

    public void HpUIOff()
    {
        StartCoroutine(UIOffTimer());
    }

    private IEnumerator UIOffTimer()
    {
        while (true)
        {

            if (destoryTimer > 0.0f)
            {
                destoryTimer -= Time.deltaTime;
                if (destoryTimer < .0f)
                {
                    destoryTimer = .0f;
                    startImgFadeOut = true;

                }
            }


            if (startImgFadeOut)
            {
                if (hpbarCover.gameObject.activeSelf)
                    hpbarCover.gameObject.SetActive(false);


                yield break;

            }

            yield return null;
        }
    }
    //public IEnumerator UnitDeadSrAlpha(Unit unit,SpriteRenderer sp)
    //{
    //    if (sp == null)
    //        unit.TryGetComponent<SpriteRenderer>(out sp);


    //    while(true)
    //    {

    //        if (destoryTimer > 0.0f)
    //        {
    //            destoryTimer -= Time.deltaTime;
    //            if (destoryTimer < .0f)
    //            {
    //                destoryTimer = .0f;
    //                startImgFadeOut = true;
                    
    //            }
    //        }


    //        if (startImgFadeOut)
    //        {
    //            if (hpbarCover.gameObject.activeSelf)
    //                hpbarCover.gameObject.SetActive(false);

    //            color = sp.color;
    //            if (color.a > .0f)
    //            {
    //                color.a -= Time.deltaTime * 2.0f;
    //            }
    //            else
    //                yield break;

    //            sp.color = color;


    //        }

    //        yield return null;
    //    }
    //}

    //public virtual void UnitDeadSpAlpha<T>(T unit, SpriteRenderer sp) where T : Unit
    //{
    //    if(sp == null)
    //        unit.TryGetComponent<SpriteRenderer>(out sp);


    //    if(destoryTimer > 0.0f)
    //    {
    //        destoryTimer -= Time.deltaTime;
    //        if(destoryTimer < .0f)
    //        {
    //            destoryTimer = .0f;
    //            startImgFadeOut = true;
    //        }
    //    }


    //    if(startImgFadeOut)
    //    {
    //        if (hpbarCover.gameObject.activeSelf)
    //            hpbarCover.gameObject.SetActive(false);

    //        color = sp.color;
    //        if (color.a > .0f)
    //        {
    //            color.a -= Time.deltaTime * 2.0f;
    //        }

    //        sp.color = color;
    //    }


    //}

    public void Notified(float hpPer)  //������ ���������� �����غ����� -> hp���� �޾ƿ��°�
    {  
        //��ü�� ü���� �ٲ𶧸��� ���⼭ Hp�� ����
        if (updateHpCo != null)
            StopCoroutine(updateHpCo);

        updateHpCo = StartCoroutine(UpdateHp(hpPer));
    }


    private IEnumerator UpdateHp(float hpPer)
    {
        if (hpbar == null)
            yield break;

        hpbar.fillAmount = hpPer;  //���� ü���ۼ�Ʈ��ŭ

    }
}
