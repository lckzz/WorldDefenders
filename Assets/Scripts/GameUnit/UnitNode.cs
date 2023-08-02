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

    [SerializeField]
    private float spawnCoolTime = 0.0f;

    private float unitCost = 0f;

    bool coolCheck = false;     //쿨타임 체크
    Color grayColor = new Color32(99, 99, 99, 255);
    
    public float UnitCost { get { return unitCost; } }
    public bool CoolCheck { get { return coolCheck; } set { coolCheck = value; } }
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
    }
}
