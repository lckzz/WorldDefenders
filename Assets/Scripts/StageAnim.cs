using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAnim : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetChild(0).TryGetComponent(out anim);
    }

    public void StageAnimSet(string animName, bool animIsOn)
    {
        if(anim != null)
        {
            anim.SetBool(animName, animIsOn);
        }
    }
}
