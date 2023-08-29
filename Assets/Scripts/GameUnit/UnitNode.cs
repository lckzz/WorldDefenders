using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitNode : MonoBehaviour
{
    private UnitClass unit = UnitClass.Warrior;
    [SerializeField]
    private Image unitImg;
    [SerializeField]
    private Image unitCoolImg;
    [SerializeField]
    private TextMeshProUGUI unitCostTxt;
    private RectTransform unitRt;

    [SerializeField]
    private float spawnCoolTime = 0.0f;

    [SerializeField] Sprite[] unitWarriorimgs;
    [SerializeField] Sprite[] unitArcherimgs;
    [SerializeField] Sprite[] unitSpearimgs;




    private float unitCost = 0f;

    bool coolCheck = false;     //쿨타임 체크
    Color grayColor = new Color32(99, 99, 99, 255);
    
    public float UnitCost { get { return unitCost; } }
    public bool CoolCheck { get { return coolCheck; } set { coolCheck = value; } }
    public UnitClass Unit { get { return unit; } }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        
        //Debug.Log(coolCheck);

        if (coolCheck)
        {
            Managers.UI.GetSceneUI<UI_GamePlay>().UpdateUnitCoolTime(unitCoolImg, 1.0f, spawnCoolTime, 1.0f);

            if (spawnCoolTime > 0.0f)
            {
                spawnCoolTime -= Time.deltaTime;
                if (spawnCoolTime <= .0f)
                {
                    coolCheck = false;
                    spawnCoolTime = 1.0f;
                }
            }
        }

        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateUnitCostEnable(unitImg,grayColor,unitCost);

    }



    void Init()
    {
        //유닛별로 맞는 데이터 연결 (이미지랑 쿨타임등등)
        unitCost = 30.0f;
        if(unitRt == null)
            TryGetComponent(out unitRt);
    }

    public void UnitInit(UnitClass unitClass)      //생성하면서 유닛노드는 유닛에 맞게 갱신
    {
        unit = unitClass;
        switch (unit)
        {
            case UnitClass.Warrior:
                if (GlobalData.g_UnitWarriorLv < 5)
                    unitImg.sprite = unitWarriorimgs[0];
                else
                    unitImg.sprite = unitWarriorimgs[1];
                break;

            case UnitClass.Archer:
                if (GlobalData.g_UnitArcherLv < 5)
                    unitImg.sprite = unitArcherimgs[0];
                else
                    unitImg.sprite = unitArcherimgs[1];
                break;

            case UnitClass.Spear:
                if (GlobalData.g_UnitSpearLv < 5)
                    unitImg.sprite = unitSpearimgs[0];
                else
                    unitImg.sprite = unitSpearimgs[1];
                break;
        }

    }

    public void UnitPositionSet(Vector3 pos)
    {
        if(unitRt == null)
            TryGetComponent(out unitRt);
        unitRt.anchoredPosition = pos;
    }

    public void NodeMove()
    {
        if (unitRt.localPosition.y == -260.0f)
            unitRt.DOLocalMoveY(-80.0f, 0.5f).SetEase(Ease.OutBack);
    }
}
