using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUnit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprend;
    [SerializeField] private UnitClass e_UnitClass = UnitClass.Count;


    public UnitClass E_UniClass { get { return e_UnitClass; } set { e_UnitClass = value; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RefreshUnitSet()
    {
        switch (e_UnitClass)
        {
            case UnitClass.Warrior:

                UnitSpriteRender(GlobalData.g_UnitWarriorLv, "KnifeUnitLv1Img", "KnifeUnitLv2Img");
                break;

            case UnitClass.Archer:
                Debug.Log($"��ó ����! {e_UnitClass}");
                UnitSpriteRender(GlobalData.g_UnitArcherLv, "BowUnitLv1Img", "BowUnitLv2Img");

                break;

            case UnitClass.Spear:
                Debug.Log($"â�� ����! {e_UnitClass}");
                UnitSpriteRender(GlobalData.g_UnitSpearLv, "SpearUnitLv1Img", "SpearUnitLv2Img");

                break;

            default:
                Debug.Log("���ֳ�忡 ����Ŭ������ ������ �ȵ̾��;");
                gameObject.SetActive(false);
                break;

        }
    }


    void UnitSpriteRender(int unitLv, string pathLv1, string pathLv2)
    {
        gameObject.SetActive(true);
        //���ֳ��UI�� �̹��� ��������Ʈ�� �ٲ��ش�.
        if (unitLv < 5)
        {
            Debug.Log(Managers.Resource.Load<Sprite>($"Sprite/{pathLv1}"));
            sprend.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv1}");

        }
        else
        {
            sprend.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv2}");

        }
    }
}
