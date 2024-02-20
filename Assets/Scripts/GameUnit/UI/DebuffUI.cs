using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;


public class DebuffUI : MonoBehaviour
{
    

    [SerializeField] private Image debuffIconImg;
    private Sprite fireIconSprite;
    private Sprite weaknessIconSprite;

    private Dictionary<DebuffType, Sprite> debuffUiDict;

    private readonly string fireDebuffSpriteStr = "Sprite/Skill/FireArrow";
    private readonly string weaknessDebuffSpriteStr = "Sprite/Skill/Weakness";


    public void UIInit(DebuffType debuffType)
    {
        fireIconSprite = Managers.Resource.Load<Sprite>(fireDebuffSpriteStr);
        weaknessIconSprite = Managers.Resource.Load<Sprite>(weaknessDebuffSpriteStr);

        debuffUiDict = new Dictionary<DebuffType, Sprite>
        {
            {DebuffType.Fire,fireIconSprite },
            {DebuffType.Weakness,weaknessIconSprite}
        };

        UIRefresh(debuffUiDict[debuffType]);

    }

    private void UIRefresh(Sprite sprite)
    {
        if (debuffIconImg == null)
            TryGetComponent(out debuffIconImg);

        debuffIconImg.sprite = sprite;
    }




    public void DebuffUIDestroy()
    {
        if(gameObject != null)
            Managers.Resource.Destroy(this.gameObject);
    }








}
