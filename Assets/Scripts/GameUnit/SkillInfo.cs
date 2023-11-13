using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SkillInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameTxt;
    [SerializeField] private TextMeshProUGUI skillInfoTxt;
    [SerializeField] private TextMeshProUGUI skillDesc;
    [SerializeField] private Image skillImg;
    [SerializeField] private GameObject skillLockObj;


    private SkillLock skillLock;


    private void Start()
    {
        skillLockObj.TryGetComponent(out skillLock);
    }

    public void SkillInfoInit(string skillName,string skillvalue,string skillCoolTime, string desc , string skillImgPath,Define.PlayerSkill playerSkillType)
    {
        skillNameTxt.text = skillName;
        skillDesc.text = desc;
        skillImg.sprite = Managers.Resource.Load<Sprite>(skillImgPath);

        skillLockObj.SetActive(false);

        switch(playerSkillType)
        {
            case Define.PlayerSkill.Heal:
                skillInfoTxt.text = $"회복량 : {skillvalue}%\n쿨타임 : {skillCoolTime}초";
                break;

            case Define.PlayerSkill.FireArrow:
                skillInfoTxt.text = $"지속시간 : {skillvalue}초\n쿨타임 : {skillCoolTime}초";
                if (Managers.Game.FireArrowSkillLv != 1)
                {
                    skillLockObj.SetActive(true);
                    skillLock.lockTextRefresh(playerSkillType);
                }

                break;

            case Define.PlayerSkill.Weakness:
                skillInfoTxt.text = $"디버프 : {skillvalue}%\n쿨타임 : {skillCoolTime}초";
                if (Managers.Game.WeaknessSkillLv != 1)
                {
                    skillLockObj.SetActive(true);
                    skillLock.lockTextRefresh(playerSkillType);
                }
                break;


        }
    }



}
