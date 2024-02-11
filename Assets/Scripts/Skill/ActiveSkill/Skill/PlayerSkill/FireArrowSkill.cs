using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrowSkill : ActiveSkill
{
    PlayerSkillData playerSkillData;
    public override void SkillDataSetting(int id)
    {
        if (Managers.Data.fireArrowSkillDict.TryGetValue(id, out PlayerSkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
        playerSkillData = SkillData as PlayerSkillData;
    }



    public override void UseSkill(PlayerController player)
    {

        //플레이어컨트롤에 접근해서 일반화살 말고 불화살 버전으로 바꿔준다.
        if (player == null)
            return;

        player.PlayerArrow = Define.PlayerArrowType.Fire;
        player.SkillOnOffPlayerFever(true);
        player.SkillData = playerSkillData;

        //이펙트 추가
    }
}
