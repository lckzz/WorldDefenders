using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUnit : MonoBehaviour
{
    [SerializeField] private GameObject lobbyObj;
    [SerializeField] private UnitClass e_UnitClass = UnitClass.Count;
    private GameObject unitPrefab;

    public UnitClass E_UniClass { get { return e_UnitClass; } set { e_UnitClass = value; } }
    // Start is called before the first frame update

    private string[] warriorStr = { "WarriorUnitLv1", "WarriorUnitLv2", "WarriorUnitLv3" };
    private string[] archerStr = { "ArcherUnitLv1", "ArcherUnitLv2", "ArcherUnitLv3" };
    private string[] spearStr = { "SpearUnitLv1", "SpearUnitLv2", "SpearUnitLv3" };
    private string[] priestStr = { "PriestUnitLv1", "PriestUnitLv2", "PriestUnitLv3" };
    private string magicianStr = "MagicianUnit";
    private string cavalryStr = "CavalryUnit";


    public void RefreshUnitSet()
    {
        if(this.gameObject.transform.childCount > 0)    //�������ٶ����� ������ ���ӿ�����Ʈ���� �������ְ� �����
        {
            for(int ii = 0; ii < gameObject.transform.childCount; ii++)
            {
                Transform goTr = gameObject.transform.GetChild(ii);

                Destroy(goTr.gameObject);
            }
        }


        switch (e_UnitClass)
        {
            case UnitClass.Warrior:

                UnitSpriteRender(e_UnitClass, GlobalData.g_UnitWarriorLv, warriorStr[0], warriorStr[1]);
                break;

            case UnitClass.Archer:
                Debug.Log($"��ó ����! {e_UnitClass}");
                UnitSpriteRender(e_UnitClass, GlobalData.g_UnitArcherLv, archerStr[0], archerStr[1]);
                break;

            case UnitClass.Spear:
                Debug.Log($"â�� ����! {e_UnitClass}");
                UnitSpriteRender(e_UnitClass, GlobalData.g_UnitSpearLv, spearStr[0], spearStr[1]);
                break;

            case UnitClass.Priest:
                Debug.Log($"���� ����! {e_UnitClass}");
                UnitSpriteRender(e_UnitClass, GlobalData.g_UnitPriestLv, priestStr[0], priestStr[1]);
                break;


            case UnitClass.Magician:
                Debug.Log($"������ ����! {e_UnitClass}");
                SpecialUnitSpriteRender(e_UnitClass, magicianStr);
                break;
            case UnitClass.Cavalry:
                Debug.Log($"�⸶�� ����! {e_UnitClass}");
                SpecialUnitSpriteRender(e_UnitClass, cavalryStr);
                break;
            default:
                Debug.Log("���ֳ�忡 ����Ŭ������ ������ �ȵ̾��;");
                gameObject.SetActive(false);
                break;

        }
    }


    void UnitSpriteRender(UnitClass uniClass, int unitLv, string pathLv1, string pathLv2)
    {
        gameObject.SetActive(true);

        switch (uniClass)
        {
            case UnitClass.Warrior:
                if (unitLv < 5)
                    unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Warrior/{pathLv1}",this.gameObject.transform);
                
                else
                    unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Warrior/{pathLv2}", this.gameObject.transform);
                
                break;
            case UnitClass.Archer:
                if (unitLv < 5)
                    unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Archer/{pathLv1}", this.gameObject.transform);

                else
                    unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Archer/{pathLv2}", this.gameObject.transform);

                break;
            case UnitClass.Spear:
                if (unitLv < 5)
                    unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Spear/{pathLv1}", this.gameObject.transform);
                else
                    unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Spear/{pathLv2}", this.gameObject.transform);
                break;

            case UnitClass.Priest:
                if (unitLv < 5)
                    unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Priest/{pathLv1}", this.gameObject.transform);
                else
                    unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Priest/{pathLv2}", this.gameObject.transform);
                break;
        }


        if (unitPrefab != null)
        {
            unitPrefab.AddComponent<LobbyUnitController>();
            unitPrefab.AddComponent<BoxCollider2D>();
            unitPrefab.tag = "Unit";
        }
    }
    void SpecialUnitSpriteRender(UnitClass uniClass, string path)
    {
        gameObject.SetActive(true);

        switch (uniClass)
        {
            case UnitClass.Magician:
                unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Magician/{path}", this.gameObject.transform);
                break;
            case UnitClass.Cavalry:
                unitPrefab = Managers.Resource.Instantiate($"Unit/LobbyUnit/Cavalry/{path}", this.gameObject.transform);
                break;

        }

        if (unitPrefab != null)
        {
            unitPrefab.AddComponent<LobbyUnitController>();
            unitPrefab.AddComponent<BoxCollider2D>();
            unitPrefab.tag = "Unit";
        }
    }
}
