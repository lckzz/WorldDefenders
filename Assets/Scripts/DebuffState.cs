using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class DebuffState : MonoBehaviour
{

    [SerializeField] private Image debuffIconImg;
    private DebuffType debuffType = DebuffType.Count;
    private Sprite iconSprite;
    public void SetDebuffState(DebuffType debuffType)
    {
        this.debuffType = debuffType;

        switch (debuffType)
        {
            case DebuffType.Fire:
                
                break;

            case DebuffType.Weakness:

                break;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
