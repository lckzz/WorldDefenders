using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    GameObject costItem;
    GameObject goldItem;
    int randIdx;

    // Start is called before the first frame update
    void Start()
    {
        costItem = Managers.Resource.Load<GameObject>("Prefabs/DropItem/Cost");
        goldItem = Managers.Resource.Load<GameObject>("Prefabs/DropItem/Gold");

    }

    public void Drop(Vector3 dropVec)
    {
        randIdx = Random.Range(0, 2);
        if (randIdx == 0)
            Managers.Resource.Instantiate(costItem, dropVec);
        else
            Managers.Resource.Instantiate(goldItem, dropVec);

    }
}
