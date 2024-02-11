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
            node.CoolCheck = true;          //ť�� �־��ָ鼭 ���� ��Ÿ�� ��
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
        if (spawnQueue.Count > 0)    //ť�� ���� �ִٸ�
            spawnQueue.Clear();         //Ŭ�������ֱ�
    }


}
