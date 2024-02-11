using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustAppear : MonoBehaviour
{
    private enum AppearUnitTag
    {
        Unit,
        Monster,
        None
    }

    [SerializeField] BoxCollider2D boxcoll;
    [SerializeField] private AppearUnitTag appearEnemyTag = AppearUnitTag.None;

    private Animator anim;
    private CameraCtrl cameraCtrl;
    // Start is called before the first frame update

    private void OnEnable()
    {
        if (anim != null)
            Init();
    }


    private void Start()
    {
        TryGetComponent(out anim);
        Camera.main.TryGetComponent(out cameraCtrl);
        Init();
    }

    private void Init()
    {
        Managers.Sound.Play("Effect/UnitDust");
        cameraCtrl.ShakeCamera(0.2f, 0.25f);
    }

    private void Update()
    {
        if (anim != null)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //해당 콜라이더에 들어오면 넉백당함

        if (coll.CompareTag(appearEnemyTag.ToString()))
        {
            if (coll.TryGetComponent(out Unit unit))
            {
                unit.OnDamage(0, 200);
            }
        }

    }
}
