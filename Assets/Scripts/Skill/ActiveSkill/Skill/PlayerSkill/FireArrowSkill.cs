using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrowSkill : ActiveSkill
{
    public override void SkillDataSetting(int id)
    {
        if (Managers.Data.fireArrowSkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
    }



    public override void UseSkill(PlayerController player)
    {


        Debug.Log("요기ㅏ에요");
        //플레이어컨트롤에 접근해서 일반화살 말고 불화살 버전으로 바꿔준다.
        if (player == null)
            return;

        player.PlayerArrow = Define.PlayerArrowType.Fire;
        player.SkillOnOffPlayerFever(true);
        player.SkillData = SkillData;

        //이펙트 추가
    }
}
