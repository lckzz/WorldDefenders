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
        //�ش� �÷��̾ų�� Ű���� ���ͼ� ���� �ִٸ� �� Ÿ�Կ� �´� �����������Ʈ�� �߰����ش�.
        if(debuffDict.TryGetValue(playerSkill, out Type debuffType))
        {
            return gameObject.AddComponent(debuffType) as Debuff;
        }


        return null;
    }
}
