using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoKnockBackZone : MonoBehaviour
{
    [SerializeField] private Define.NoKnockBackType noKnockBackType;
    private Unit unit;
    private void OnTriggerStay2D(Collider2D coll)
    {
        if(noKnockBackType == Define.NoKnockBackType.Unit)
        {
            if (coll != null && coll.tag.Contains("Unit"))
            {
                Debug.Log("여기 들어와있어용");
                coll.TryGetComponent(out unit);
                unit.IsNoKnockBack = true;
            }
        }

        else
        {
            if (coll != null && coll.tag.Contains("Monster"))
            {
                coll.TryGetComponent(out unit);
                unit.IsNoKnockBack = true;
            }
        }



    }


    private void OnTriggerExit2D(Collider2D coll)
    {
        if (noKnockBackType == Define.NoKnockBackType.Unit)
        {
            if (coll != null && coll.tag.Contains("Unit"))
            {
                Debug.Log("여기 나갓어용");

                coll.TryGetComponent(out unit);
                unit.IsNoKnockBack = false;
            }
        }

        else
        {
            if (coll != null && coll.tag.Contains("Monster"))
            {
                coll.TryGetComponent(out unit);
                unit.IsNoKnockBack = false;
            }
        }
    }
}
