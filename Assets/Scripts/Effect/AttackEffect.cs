using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out anim);

    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            //this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        //if(anim != null)
        //    anim.Play("AttackEff");
    }
}
