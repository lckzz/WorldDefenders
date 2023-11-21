using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class SkillOpenNotice : MonoBehaviour
{
    [SerializeField] private Image skillImg;
    [SerializeField] private TextMeshProUGUI openSkillTxt;
    [SerializeField] private GameObject touchClickObj;
    [SerializeField] private RectTransform imgRt;

    private float minHeight = 53.0f;

    private float maxHeight = 265.0f;
    private Vector2 maxRtHeight;

    private bool isStart = false;       //클릭할수 있는지 판별하는 변수

    [SerializeField] private PlayerSkill playerSkill;

    private readonly string fireArrowSkillOpenStr = "폭발 화살 스킬이 개방 되었습니다.";
    private readonly string WeaknessSkillOpenStr = "약화 스킬이 개방 되었습니다.";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (touchClickObj == null || imgRt == null)
            return;
        if(touchClickObj.activeSelf == true)
            touchClickObj.SetActive(false);

        isStart = false;

        SkillImgSetting();

        maxRtHeight = new Vector2(imgRt.sizeDelta.x, maxHeight);
        imgRt.sizeDelta = new Vector2(imgRt.sizeDelta.x, minHeight);
        imgRt.DOSizeDelta(maxRtHeight, 0.2f).SetEase(Ease.Linear).OnComplete(ImageRtComplete).SetDelay(0.5f);
    }



    // Update is called once per frame
    void Update()
    {
        if (isStart == false)
            return;


        if(Input.GetMouseButtonDown(0))
        {
            //클릭시 해당 오브젝트 꺼짐
            this.gameObject.SetActive(false);
        }
    }


    private void ImageRtComplete()
    {
        //터치판넬이 다 펴지면 
        touchClickObj.SetActive(true);
        isStart = true;
    }

    private void SkillImgSetting()
    {

    }


    public void SetPlayerSkillInfo(PlayerSkill playerSkill)
    {
        this.playerSkill = playerSkill;

        if (this.playerSkill == PlayerSkill.FireArrow)
        {
            skillImg.sprite = Managers.Resource.Load<Sprite>("Sprite/Skill/FireArrow");
            openSkillTxt.text = fireArrowSkillOpenStr;
        }
        else if (this.playerSkill == PlayerSkill.Weakness)
        {
            skillImg.sprite = Managers.Resource.Load<Sprite>("Sprite/Skill/Weakness");
            openSkillTxt.text = WeaknessSkillOpenStr;
        }
    }
}
