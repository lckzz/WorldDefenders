using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class SkillNode : MonoBehaviour
{
    PlayerSkill playerSkillType = PlayerSkill.Count;
    public PlayerSkill PlayerSkillType { get { return playerSkillType; } set { playerSkillType = value;  } }
    //스킬윈도우를 초기화할때 enum의 순서대로 타입을 셋해줄예정

    PlayerSkillState playerSkillSt = PlayerSkillState.NonEquip;
    public PlayerSkillState PlayerSkillSt { get { return playerSkillSt; } }

    [SerializeField] private GameObject selectImg;
    [SerializeField] private Image skillImg;
    [SerializeField] private Image equipSkillImg;



    RectTransform rt;

    Vector3[] pos = new Vector3[(int)PlayerSkill.Count];
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


    public void SkillNodeSetting(PlayerSkill type)
    {
        rt.anchoredPosition = pos[(int)type];
        switch(type)
        {
            case PlayerSkill.Heal:
                skillData = Managers.Data.healSkillDict[Managers.Game.TowerHealSkillLv];
                break;
            case PlayerSkill.FireArrow:
                skillData = Managers.Data.fireArrowSkillDict[Managers.Game.FireArrowSkillLv];
                break;
            case PlayerSkill.Weakness:
                skillData = Managers.Data.weaknessSkillDict[Managers.Game.WeaknessSkillLv];
                break;
        }

        skillImg.sprite = Managers.Resource.Load<Sprite>(skillData.skillImg);

        if (skillData.level <= 0)
            skillImg.color = colors[0];
        else
            skillImg.color = colors[1];

        SkillNodeRefresh(type);

    }

    public void SkillNodeRefresh(PlayerSkill type)   //스킬 장착시 갱신
    {
        if(playerSkillSt == PlayerSkillState.NonEquip)      //장착중인 상태가 아닐때
        {
            if (Managers.Game.CurPlayerEquipSkill == type)   //장착한 스킬이랑 같다면 장착중표시를 켜주고 상태를 장착상태로 바꿔준다.
            {
                equipSkillImg.gameObject.SetActive(true);
                playerSkillSt = PlayerSkillState.Equip;
            }
        }
        else//장착중인 상태라면
        {
            if (Managers.Game.CurPlayerEquipSkill != type)   //장착한 스킬이랑 다르면 장착중표시를 꺼주고 상태를 비장착상태로 바꿔준다.
            {
                equipSkillImg.gameObject.SetActive(false);
                playerSkillSt = PlayerSkillState.NonEquip;
            }
        }


    }


    public void ClickNodeSelectImgOnOff(bool onoff)
    {
        selectImg.SetActive(onoff);
    }

}
