using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    [SerializeField] private RectTransform titleLogoRt;
    [SerializeField] private TextMeshProUGUI touchTxt;
    [SerializeField] private GameObject touchPanel;


    private TitleTouch titleTouch;
    //��ġ 
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    //��ġ




    // Start is called before the first frame update
    void Start()
    {
        Managers.Sound.Play("BGM/MainTheme", Define.Sound.BGM);
        LoadGameData();
        TitleSoundInit();
        GameObject canvas = GameObject.Find("Canvas");
        canvas?.TryGetComponent(out gr);

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);


        titleLogoRt.DOSizeDelta(new Vector2(900.0f, 900.0f), 0.5f).SetEase(Ease.OutBounce);
        touchTxt.DOFade(0, 1f).SetLoops(-1,LoopType.Yoyo);



    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Managers.Sound.Play("Effect/UI_Click");
            titleTouch = UiRaycastGetFirstComponent<TitleTouch>(gr);
            touchTxt.gameObject.SetActive(false);
            titleTouch.TouchOnPanel();


            Managers.Game.UnitLvDictRefresh();


        }
    }

    void LoadGameData()
    {
        if (Managers.Game.FileLoad())
        {
            //���������� �ִٸ�
            Managers.Game.GameDataInit();
            Managers.Game.FileLoad();   //������ �ҷ��´�
            if (Managers.Game.UnitWarriorLv == 0)        //������ �ҷ��Դµ� �⺻ĳ������ ���簡 ������ 0�̶��
            {
                Managers.Game.GameDataInit();       //�̻��ϰ� ����� �������Ϸ� �ʱ�ȭ �����ش�.
                Managers.Game.SlotUnitClass.Clear();        //�迭�� �ʱ�ȭ ��������Ѵ�.
            }


            //�ʱ�ȭ�� ���������� �ҷ���
        }
        else
        {
            //���������� ���ٸ�
            Managers.Game.GameDataInit(); //�ʱ�ȭ��

        }

    }

    void TitleSoundInit()
    {
        Managers.Sound.SoundValue(Define.Sound.BGM, Managers.Game.BgmValue);
        Managers.Sound.SoundValue(Define.Sound.Effect, Managers.Game.EffValue);

    }

    T UiRaycastGetFirstComponent<T>(GraphicRaycaster gr) where T : Component
    {
        rrList.Clear();

        gr.Raycast(ped, rrList);
        if (rrList.Count == 0)
            return null;



        return rrList[0].gameObject.GetComponent<T>();
    }

    public override void Clear()
    {
        
    }
}
