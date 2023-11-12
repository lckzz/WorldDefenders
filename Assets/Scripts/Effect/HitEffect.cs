using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
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
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            StartCoroutine(Util.DestroyTime(gameObject, 0.2f));
        }
    }

    private void OnEnable()
    {
        
    }
}
