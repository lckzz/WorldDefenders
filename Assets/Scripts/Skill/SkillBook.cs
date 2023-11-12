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
            activeSkillList[0].SkillDataSetting(Managers.Game.UnitMagicianLv);

            return mk as T;
        }
        else if (type == typeof(SwordSummonSkill))
        {
            skillPrefab.TryGetComponent(out SwordSummonSkill ssk);
            activeSkillList.Add(ssk);
            activeSkillList[0].SkillDataSetting(1);


            return ssk as T;
        }

        else if (type == typeof(SwordDanceSkill))
        {
            skillPrefab.TryGetComponent(out SwordDanceSkill sk);
            activeSkillList.Add(sk);
            activeSkillList[0].SkillDataSetting(GlobalData.g_EliteWarriorID);


            return sk as T;
        }

        else if (type == typeof(SkeletonSummonSkill))
        {
            skillPrefab.TryGetComponent(out SkeletonSummonSkill ssk);
            activeSkillList.Add(ssk);
            activeSkillList[0].SkillDataSetting(GlobalData.g_EliteCavalryID);


            return ssk as T;
        }

        else if (type == typeof(DarkPowerSkill))
        {
            skillPrefab.TryGetComponent(out DarkPowerSkill dps);
            activeSkillList.Add(dps);
            activeSkillList[0].SkillDataSetting(GlobalData.g_EliteShamanID);


            return dps as T;
        }


        else if(type == typeof(TowerHealSkill))
        {
            skillPrefab = Managers.Resource.Load<GameObject>("Prefabs/Skill/TowerHealSkill");
            skillPrefab.TryGetComponent(out TowerHealSkill thk);
            activeSkillList.Add(thk);
            activeSkillList[0].SkillDataSetting(GlobalData.g_SkillHealLv);

        }
        else if (type == typeof(FireArrowSkill))
        {
            skillPrefab = Managers.Resource.Load<GameObject>("Prefabs/Skill/FireArrowSkill");
            skillPrefab.TryGetComponent(out FireArrowSkill fak);
            activeSkillList.Add(fak);
            activeSkillList[0].SkillDataSetting(GlobalData.g_SkillFireArrowLv);

        }
        else if (type == typeof(WeaknessSkill))
        {
            skillPrefab = Managers.Resource.Load<GameObject>("Prefabs/Skill/WeaknessSkill");
            skillPrefab.TryGetComponent(out WeaknessSkill wsk);
            activeSkillList.Add(wsk);
            activeSkillList[0].SkillDataSetting(GlobalData.g_SkillWeaknessLv);

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
