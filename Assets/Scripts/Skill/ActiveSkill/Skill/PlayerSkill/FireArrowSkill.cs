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

        //�÷��̾���Ʈ�ѿ� �����ؼ� �Ϲ�ȭ�� ���� ��ȭ�� �������� �ٲ��ش�.
        if (player == null)
            return;

        player.PlayerArrow = Define.PlayerArrowType.Fire;
        player.SkillOnOffPlayerFever(true);
        player.SkillData = playerSkillData;

        //����Ʈ �߰�
    }
}
