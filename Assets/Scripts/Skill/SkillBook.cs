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

            return mk as T;
        }
        else if (type == typeof(SwordDanceSkill))
        {
            skillPrefab.TryGetComponent(out SwordDanceSkill sk);
            activeSkillList.Add(sk);

            return sk as T;
        }

        return null;
    }

}
