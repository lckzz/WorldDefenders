using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnitNodeUI : UI_BaseSettingUnit
{
    //유닛배치에 붙어있는 유닛노드관한 스크립트
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



        if(e_UnitClass >= UnitClass.Magician)  //스페셜유닛이라면
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

        if (unitPosObj != null)  //유닛노드의 위치할 오브젝트가 있다면
            Managers.Resource.Instantiate(unitStat.unitSpriteUIPrefabs, unitPosObj.transform);        //유닛노드의 위치한 오브젝트의 하위에 유닛을 생성한다.
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
    //    //유닛노드UI의 이미지 스프라이트를 바꿔준다.
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
