using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    Define.PlayerSkill playerSkillType = Define.PlayerSkill.Count;
    public Define.PlayerSkill PlayerSkillType { get { return playerSkillType; } set { playerSkillType = value;  } }
    //스킬윈도우를 초기화할때 enum의 순서대로 타입을 셋해줄예정

    [SerializeField] private GameObject selectImg;
    [SerializeField] private Image skillImg;


    RectTransform rt;

    Vector3[] pos = new Vector3[(int)Define.PlayerSkill.Count];
    Color[] colors = new Color[2];

    SkillData skillData;

    public SkillData SkillData { get { return skillData; } }


    // Start is called before the first frame update
    void Awake()
    {
        Init();


    }

    // Update is called once per frame
    void Init()
    {
        skillData = new SkillData();



        TryGetComponent<RectTransform>(out rt);

        for(int ii = 0; ii < pos.Length; ii++)
        {
            if (ii == 0)
                pos[ii] = new Vector3(-230.0f, 0.0f, 0.0f);
            else if(ii == 1)
                pos[ii] = new Vector3(-0.0f, 0.0f, 0.0f);
            else
                pos[ii] = new Vector3(230.0f, 0.0f, 0.0f);

        }

        for(int i = 0; i < colors.Length; i++)
        {
            if (i == 0)
                colors[i] = new Color32(75, 75, 75, 255);
            else
                colors[i] = new Color32(255, 255, 255, 255);
        }

    }


    public void SkillNodeSetting(Define.PlayerSkill type)
    {
        rt.anchoredPosition = pos[(int)type];
        switch(type)
        {
            case Define.PlayerSkill.Heal:
                skillData = Managers.Data.healSkillDict[Managers.Game.TowerHealSkillLv];
                break;
            case Define.PlayerSkill.FireArrow:
                skillData = Managers.Data.fireArrowSkillDict[Managers.Game.FireArrowSkillLv];
                break;
            case Define.PlayerSkill.Weakness:
                skillData = Managers.Data.weaknessSkillDict[Managers.Game.WeaknessSkillLv];
                break;
        }

        skillImg.sprite = Managers.Resource.Load<Sprite>(skillData.skillImg);

        if (skillData.level <= 0)
            skillImg.color = colors[0];
        else
            skillImg.color = colors[1];

    }


    public void ClickNodeSelectImgOnOff(bool onoff)
    {
        selectImg.SetActive(onoff);
    }

}
