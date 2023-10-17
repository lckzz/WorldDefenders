using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField]
    protected Image hpbar;
    [SerializeField]
    protected Image hpbarCover;
    protected float minHp = .0f;
    [SerializeField] protected float hpSpeed = 1.7f;
    protected SpriteRenderer spr;
    protected Color color = new Color(0, 0, 0);

    protected float destoryTimer = 1.5f;
    protected bool startImgFadeOut = false;
    protected float hp;


    private void OnEnable()
    {
        destoryTimer = 1.5f;
        startImgFadeOut = false;
        hpbarCover.gameObject.SetActive(true);
        hpbar.fillAmount = 1.0f;
    }


    protected T ComponentInit<T>(out T compo) where T : Component
    {
        if(TryGetComponent<T>(out compo))
            return compo;
        else
            return null;
    }

    public virtual void UpdateHp(float hpspeed) { }


    public virtual void UnitDeadSpAlpha<T>(T unit, SpriteRenderer sp) where T : Unit
    {
        if(sp == null)
            unit.TryGetComponent<SpriteRenderer>(out sp);


        if(destoryTimer > 0.0f)
        {
            destoryTimer -= Time.deltaTime;
            if(destoryTimer < .0f)
            {
                destoryTimer = .0f;
                startImgFadeOut = true;
            }
        }


        if(startImgFadeOut)
        {
            if (hpbarCover.gameObject.activeSelf)
                hpbarCover.gameObject.SetActive(false);

            color = sp.color;
            if (color.a > .0f)
            {
                color.a -= Time.deltaTime * 2.0f;
            }

            sp.color = color;
        }


    }


}
