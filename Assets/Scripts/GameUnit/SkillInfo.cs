using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SkillInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameTxt;
    [SerializeField] private TextMeshProUGUI curSkillLvTxt;
    [SerializeField] private TextMeshProUGUI skillDesc;
    [SerializeField] private Image skillImg;


    public void SkillInfoInit(string skillName, string curSkillLv, string desc , string skillImgPath)
    {
        skillNameTxt.text = skillName;
        curSkillLvTxt.text = "Lv " + curSkillLv;
        skillDesc.text = desc;
        skillImg.sprite = Managers.Resource.Load<Sprite>(skillImgPath);
    }



}
