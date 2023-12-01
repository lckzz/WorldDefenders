using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DebuffCreator : MonoBehaviour
{

    private Dictionary<PlayerSkill, Type> debuffDict = new Dictionary<PlayerSkill, Type>()
    {
        {PlayerSkill.FireArrow,typeof(FireDebuff) },
        {PlayerSkill.Weakness,typeof(WeaknessDebuff) },
    };

    public Debuff AddDebuffComponent(PlayerSkill playerSkill)
    {
        //해당 플레이어스킬의 키값이 들어와서 값이 있다면 그 타입에 맞는 디버프컴포넌트를 추가해준다.
        if(debuffDict.TryGetValue(playerSkill, out Type debuffType))
        {
            return gameObject.AddComponent(debuffType) as Debuff;
        }


        return null;
    }
}
