using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillLock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lockText;
    // Start is called before the first frame update

    public void lockTextRefresh(Define.PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case Define.PlayerSkill.FireArrow:
                lockText.text = "����\n�÷��̾� ���� 3";
                break;

            case Define.PlayerSkill.Weakness:
                lockText.text = "����\n�÷��̾� ���� 7";
                break;
        }

    }
}
