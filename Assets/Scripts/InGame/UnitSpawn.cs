using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class UnitSpawn 
{
    private Queue<GameObject> spawnQueue = new Queue<GameObject>();

    public Queue<GameObject> SpawnQueue { get { return spawnQueue; } }

    public void UnitSummonEnqueue(GameObject unitObj, float unitcost,UnitNode node)
    {
        if (Managers.Game.CostCheck(unitcost))
        {
            Managers.Game.CostUse(unitcost);
            spawnQueue.Enqueue(unitObj);
            node.CoolCheck = true;          //큐에 넣어주면서 유닛 쿨타임 온
        }
    }

    public void UnitSummonDequeue(GameObject go, Transform[] tr)
    {
        if (spawnQueue.Count <= 0)
            return;


        go = spawnQueue.Dequeue();
        GameObject instancObj = Managers.Resource.Instantiate(go);

        int ran = UnityEngine.Random.Range(0, 3);
        instancObj.transform.position = tr[ran].position;
    }

    public void UnitQueueClear()
    {
        if (spawnQueue.Count > 0)    //큐에 무언가 있다면
            spawnQueue.Clear();         //클리어해주기
    }


}
