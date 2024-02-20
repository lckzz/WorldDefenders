using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
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
    private Image unitBgImg;
    private RectTransform unitRt;
    private RectTransform unitImgRt;
    [SerializeField] private Button unitSpawnBtn;
    [SerializeField] private Sprite uniqueUnitSprite;


    [SerializeField]
    private float spawnCoolTime = 0.0f;
    private float maxSpawnCoolTime;

    [SerializeField] Sprite[] unitWarriorimgs;
    [SerializeField] Sprite[] unitArcherimgs;
    [SerializeField] Sprite[] unitSpearimgs;
    [SerializeField] Sprite[] unitPriestimgs;

    [SerializeField] Sprite unitMagicianimg;
    [SerializeField] Sprite unitCavalryimg;

    private Vector3 cavalryVec = new Vector3(0.0f, -19.6f, 0.0f);
    private Vector3 magicianVec = new Vector3(0.0f, -1.28f, 0.0f);



    UnitStat unitStat;
    private float unitCost = 0f;
    private GameObject unitObj;

    bool coolCheck = false;     //쿨타임 체크
    Color grayColor = new Color32(99, 99, 99, 255);
    
    public float UnitCost { get { return unitCost; } }
    public bool CoolCheck { get { return coolCheck; } set { coolCheck = value; } }
    public UnitClass Unit { get { return unit; } }
    // Start is called before the first frame update
    void Start()
    {
        Init();


        if (unitSpawnBtn != null)
            unitSpawnBtn.onClick.AddListener(UnitSummonBtnClick);
    }

    // Update is called once per frame
    void Update()
    {

        
        //Debug.Log(coolCheck);

        if (coolCheck)
        {
            Managers.UI.GetSceneUI<UI_GamePlay>().UpdateUnitCoolTime(unitCoolImg, 1.0f, spawnCoolTime, maxSpawnCoolTime);

            if (spawnCoolTime > 0.0f)
            {
                spawnCoolTime -= Time.deltaTime;
                if (spawnCoolTime <= .0f)
                {
                    coolCheck = false;
                    spawnCoolTime = 3.0f;
                }
            }
        }

        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateUnitCostEnable(unitImg,grayColor,unitCost,Managers.Game.Cost);

    }



    void Init()
    {
        //유닛별로 맞는 데이터 연결 (이미지랑 쿨타임등등)

        maxSpawnCoolTime = spawnCoolTime;
        //unitCost = 30.0f;
        if(unitRt == null)
            TryGetComponent(out unitRt);
        if (unitImgRt == null)
            unitImg?.TryGetComponent(out unitImgRt);

    }

    //유닛 버튼을 누를시 소환
    private void UnitSummonBtnClick()
    {
        if (Managers.Game.GameEndResult())       //개임이 끝났으면 유닛버튼은 눌리지 않는다.
            return;

        unitObj = Managers.Resource.Load<GameObject>($"Prefabs/{unitStat.unitInGamePrefabs}");


        if (coolCheck == false) //쿨타임이 돌고 있지않을 때
        {
            Managers.Game.UnitSummonEnqueue(unitObj, unitCost, this);
            Managers.Sound.Play("Effect/UI_Click");
        }
        else
        {
            Managers.Sound.Play("Effect/Error");

        }

    }

    public void UnitInit(UnitClass unitClass)      //생성하면서 유닛노드는 유닛에 맞게 갱신
    {
        unit = unitClass;
        unitStat = new UnitStat();

        if(Managers.Game.UnitStatDict.TryGetValue(unitClass,out Dictionary<int, UnitStat> unitStatDict))
            unitStat = unitStatDict[Managers.Game.GetUnitLevel(unitClass)];

        unitImg.sprite = Managers.Resource.Load<Sprite>($"Prefabs/{unitStat.unitSprite}");
        unitCost = unitStat.cost;
        unitCostTxt.text = unitCost.ToString();


        if (unitImgRt == null)
            unitImg?.TryGetComponent(out unitImgRt);


        if (unitBgImg == null)
            TryGetComponent(out unitBgImg);


        if (unitClass == UnitClass.Cavalry)
        {
            unitImgRt.anchoredPosition = cavalryVec;
            unitBgImg.sprite = uniqueUnitSprite;
        }

        else if(unitClass == UnitClass.Magician)
        {
            unitImgRt.anchoredPosition = magicianVec;
            unitBgImg.sprite = uniqueUnitSprite;

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
