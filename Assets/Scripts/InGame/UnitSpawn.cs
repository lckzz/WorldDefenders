using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class UnitSpawn 
{
    private Queue<GameObject> gameQueue = new Queue<GameObject>();

    public Queue<GameObject> GameQueue { get { return gameQueue; } }

    public void UnitSummonEnqueue(GameObject unitObj, float unitcost,UnitNode node)
    {
        if (Managers.Game.CostCheck(unitcost))
        {
            Managers.Game.CostUse(unitcost);
            gameQueue.Enqueue(unitObj);
            node.CoolCheck = true;          //ť�� �־��ָ鼭 ���� ��Ÿ�� ��
        }
    }

    public void UnitSummonDequeue(GameObject go, Transform[] tr)
    {
        if (gameQueue.Count <= 0)
            return;


        go = gameQueue.Dequeue();
        GameObject instancObj = Managers.Resource.Instantiate(go);

        int ran = UnityEngine.Random.Range(0, 3);
        instancObj.transform.position = tr[ran].position;
    }



}
