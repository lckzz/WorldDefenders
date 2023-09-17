using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{

    public List<ActiveSkill> activeSkillList = new List<ActiveSkill>();
    public List<PassiveSkill> passiveSkillList = new List<PassiveSkill>();
    [SerializeField] private GameObject skillPrefab;

    public T AddSkill<T>() where T : SkillBase
    {
        System.Type type = typeof(T);

        if (type == typeof(MeteroSkill))
        {
            skillPrefab.TryGetComponent(out MeteroSkill mk);
            activeSkillList.Add(mk);
            activeSkillList[0].SkillDataSetting(GlobalData.g_UnitMagicianLv);

            return mk as T;
        }
        else if (type == typeof(SwordDanceSkill))
        {
            skillPrefab.TryGetComponent(out SwordDanceSkill sk);
            activeSkillList.Add(sk);
            activeSkillList[0].SkillDataSetting(GlobalData.g_EliteWarriorID);


            return sk as T;
        }

        return null;
    }


    public T GetSkill<T>() where T : SkillBase
    {
        if(activeSkillList.Count > 0)
        {
            return activeSkillList[0] as T;

        }
        if (passiveSkillList.Count > 0)
        {
            return passiveSkillList[0] as T;

        }

        return null;
    }
}
