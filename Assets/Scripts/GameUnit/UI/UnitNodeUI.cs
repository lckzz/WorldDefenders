using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnitNodeUI : UI_BaseSettingUnit
{
    //���ֹ�ġ�� �پ��ִ� ���ֳ����� ��ũ��Ʈ
    //[SerializeField] private UnitClass e_UnitClass = UnitClass.Count;
    [SerializeField] private Image unitFrameImg;
    [SerializeField] private Sprite specialUnitSprite;

    private TextMeshProUGUI unitLvTxt;
    Vector2 spearSizeDelta = new Vector2(120.0f, 120.0f);
    Vector3 spearTr = new Vector3(-9.0f, -9.0f, 0.0f);
    private Define.UnitNodeState unitNodeState = Define.UnitNodeState.None;
    private Color32 normalClassColor = new Color32(72, 72, 72, 255);
    private Color32 specialClassColor = new Color32(121, 81, 212,255);
    private Image nodeImg;
    private IOpenPanel openPanel;

    public UnitClass E_UnitClass { get { return e_UnitClass; } set { e_UnitClass = value; } }


    protected override void Init()
    {
        base.Init();
        TryGetComponent(out nodeImg);
        for(int ii = 0; ii < transform.childCount; ii++)
        {
            if (transform.GetChild(ii).name.Contains("Lv"))
                transform.GetChild(ii).TryGetComponent(out unitLvTxt);
        }

        UnitUISpriteInit();



        if(e_UnitClass >= UnitClass.Magician)  //����������̶��
        {
            unitFrameImg.sprite = specialUnitSprite;
        }



    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    protected override void UnitUISpriteInit()
    {
        base.UnitUISpriteInit();

        if (unitPosObj != null)  //���ֳ���� ��ġ�� ������Ʈ�� �ִٸ�
            Managers.Resource.Instantiate(unitStat.unitSpriteUIPrefabs, unitPosObj.transform);        //���ֳ���� ��ġ�� ������Ʈ�� ������ ������ �����Ѵ�.
    }



    public void OpenUnitInfoTween()
    {
        openPanel = Managers.UI.PeekPopupUI<UI_UnitInfoSelectPopUp>();
        openPanel?.OpenRectTransformScaleSet();
    }



    //private void Update()
    //{
        
    //}


    //private void UnitNodeUISpriteInit(int unitLv, string pathLv1, string pathLv2)
    //{
    //    //���ֳ��UI�� �̹��� ��������Ʈ�� �ٲ��ش�.
    //    if (unitLv < 5)
    //        unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv1}");
    //    else
    //        unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv2}");
    //}


    public UnitClass SetUnitClass(UnitClass uniclass)
    {
        e_UnitClass = uniclass;
        return e_UnitClass;
    }


    public void CheckNodeEquip(Define.UnitNodeState type)
    {
        unitNodeState = type;  

        if (unitNodeState == Define.UnitNodeState.Equip)
            Destroy(this.gameObject);
    }


}
