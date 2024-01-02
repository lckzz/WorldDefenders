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


    private readonly string healSkillStr = "���� ���� ��ų�� �����Ǿ����ϴ�.";
    private readonly string fireSkillStr = "���� ȭ�� ��ų�� �����Ǿ����ϴ�.";
    private readonly string weaknessSkillStr = "��ȭ ��ų�� �����Ǿ����ϴ�.";

    private PlayerSkill playerSkill;
    private Dictionary<int, string> playerSkillDict;



    private void OnEnable()
    {
        if(playerSkillDict == null)      //��ųʸ��� null�̸�
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
