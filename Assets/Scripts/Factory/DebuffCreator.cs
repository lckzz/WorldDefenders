using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DebuffCreator : MonoBehaviour
{

    private Dictionary<PlayerSkill, Type> debuffMapping = new Dictionary<PlayerSkill, Type>()
    {
        {PlayerSkill.FireArrow,typeof(FireDebuff) },
        {PlayerSkill.Weakness,typeof(WeaknessDebuff) },
    };

    public Debuff AddDebuffComponent(PlayerSkill playerSkill)
    {

        if(debuffMapping.TryGetValue(playerSkill, out Type debuffType))
        {
            return gameObject.AddComponent(debuffType) as Debuff;
        }


        return null;
    }
}
