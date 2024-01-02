using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class SkillEquipNotice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillEquipTxt;
    [SerializeField] private Button skillEquipCheckBtn;


    private readonly string healSkillStr = "성벽 수리 스킬이 장착되었습니다.";
    private readonly string fireSkillStr = "폭발 화살 스킬이 장착되었습니다.";
    private readonly string weaknessSkillStr = "약화 스킬이 장착되었습니다.";

    private PlayerSkill playerSkill;
    private Dictionary<int, string> playerSkillDict;



    private void OnEnable()
    {
        if(playerSkillDict == null)      //딕셔너리가 null이면
        {
            playerSkillDict = new Dictionary<int, string>
            {
                { (int)PlayerSkill.Heal, healSkillStr },
                { (int)PlayerSkill.FireArrow, fireSkillStr },
                { (int)PlayerSkill.Weakness, weaknessSkillStr }

            };
        }

        playerSkill = Managers.Game.CurPlayerEquipSkill;

        SkillNoticeStringSet(playerSkillDict[(int)playerSkill]);
    }

    private void Start()
    {
        skillEquipCheckBtn?.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
    }


    private void SkillNoticeStringSet(string skillStr)
    {
        skillEquipTxt.text = skillStr;
    }
}
