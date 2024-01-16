using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEff : MonoBehaviour
{
    // Start is called before the first frame update
    public void EffectSound()
    {
        Managers.Sound.Play("Sounds/Effect/Explosion");
    }

    public void EffectBigSound()
    {
        Managers.Sound.Play("Sounds/Effect/BigExplosion");

    }
}
