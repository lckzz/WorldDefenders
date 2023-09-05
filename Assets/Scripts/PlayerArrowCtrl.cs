using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowCtrl : MonoBehaviour
{
    private PlayerController player;
    private Vector3 dir;
    private float speed = 35.0f;
    private bool arrowHit = false;
    private Animator anim;
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("Player");
        obj.TryGetComponent<PlayerController>(out player);
        player.ArrowInfo(ref dir);
        TryGetComponent<Animator>(out anim);
        TryGetComponent<Collider2D>(out col);


    }

    // Update is called once per frame
    void Update()
    {
     
        ArrowMove();
    }


    void ArrowMove()
    {
        if(!arrowHit)
            this.transform.position += dir * Time.deltaTime * speed;
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.tag.Contains("Monster"))
        {

            Unit monctrl = null;
            coll.TryGetComponent<Unit>(out monctrl);
            if (monctrl != null)
                monctrl.OnDamage(player.Att);

            Managers.Resource.ResourceEffectAndSound(monctrl.transform.position, "ArrowHit", "HitEff");

        }

        if(coll.tag.Contains("Ground"))
        {
            if(!arrowHit)
            {
                anim.SetTrigger("Hit");

                arrowHit = true;
                col.enabled = false;


                Destroy(this.gameObject, 2.0f);
            }

        }
    }
}
