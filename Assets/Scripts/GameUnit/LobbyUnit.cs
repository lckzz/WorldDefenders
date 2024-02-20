using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUnit : MonoBehaviour
{
    [SerializeField] private GameObject lobbyObj;
    [SerializeField] private UnitClass e_UnitClass = UnitClass.Count;
    private GameObject unitPrefab;
    private UnitStat unitStat = null;
    public UnitClass E_UniClass { get { return e_UnitClass; } set { e_UnitClass = value; } }
    // Start is called before the first frame update


    public void RefreshUnitSet()
    {
        if (unitStat == null)
            unitStat = new UnitStat();


        if(this.gameObject.transform.childCount > 0)    //�������ٶ����� ������ ���ӿ�����Ʈ���� �������ְ� �����
        {
            for(int ii = 0; ii < gameObject.transform.childCount; ii++)
            {
                Transform goTr = gameObject.transform.GetChild(ii);

                Destroy(goTr.gameObject);
            }
        }

        if (e_UnitClass == UnitClass.Count)
            return;


        if (Managers.Game.UnitStatDict.TryGetValue(e_UnitClass, out Dictionary<int, UnitStat> stat))
            unitStat = stat[Managers.Game.GetUnitLevel(e_UnitClass)];
        

        LobbyUnitSetting();


    }


    void LobbyUnitSetting()
    {
        gameObject.SetActive(true);
        unitPrefab = Managers.Resource.Instantiate(unitStat.unitLobbyPrefabs, this.gameObject.transform);

        if (unitPrefab != null)
            unitPrefab.tag = "Unit";
        
    }

}
