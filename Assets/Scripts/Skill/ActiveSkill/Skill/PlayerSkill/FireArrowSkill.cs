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


        Debug.Log("��⤿����");
        //�÷��̾���Ʈ�ѿ� �����ؼ� �Ϲ�ȭ�� ���� ��ȭ�� �������� �ٲ��ش�.
        if (player == null)
            return;

        player.PlayerArrow = Define.PlayerArrowType.Fire;
        player.SkillOnOffPlayerFever(true);
        player.SkillData = SkillData;

        //����Ʈ �߰�
    }
}
