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

    private bool isStart = false;       //Ŭ���Ҽ� �ִ��� �Ǻ��ϴ� ����

    [SerializeField] private PlayerSkill playerSkill;

    private readonly string fireArrowSkillOpenStr = "���� ȭ�� ��ų�� ���� �Ǿ����ϴ�.";
    private readonly string WeaknessSkillOpenStr = "��ȭ ��ų�� ���� �Ǿ����ϴ�.";


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
            //Ŭ���� �ش� ������Ʈ ����
            this.gameObject.SetActive(false);
        }
    }


    private void ImageRtComplete()
    {
        //��ġ�ǳ��� �� ������ 
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
