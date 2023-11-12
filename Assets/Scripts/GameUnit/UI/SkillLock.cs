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
                lockText.text = "조건\n플레이어 레벨 3";
                break;

            case Define.PlayerSkill.Weakness:
                lockText.text = "조건\n플레이어 레벨 7";
                break;
        }

    }
}
