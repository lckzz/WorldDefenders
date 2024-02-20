using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out animator);
    }


    public void AnimatiorEndObjectSetOff()
    {
        this.gameObject.SetActive(false);
    }
}
