using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private RectTransform titleLogoRt;
    [SerializeField] private TextMeshProUGUI touchTxt;
    [SerializeField] private GameObject touchPanel;


    private TitleTouch titleTouch;
    //터치 
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    //터치




    // Start is called before the first frame update
    void Start()
    {
        Managers.Sound.Play("BGM/TitleBGM", Define.Sound.BGM);
        GameObject canvas = GameObject.Find("Canvas");
        canvas?.TryGetComponent(out gr);

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);


        titleLogoRt.DOSizeDelta(new Vector2(900.0f, 900.0f), 0.5f).SetEase(Ease.OutBounce);
        touchTxt.DOFade(0, 1f).SetLoops(-1,LoopType.Yoyo);

        Debug.Log(Description(10, 10, 2, 2));

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            titleTouch = UiRaycastGetFirstComponent<TitleTouch>(gr);
            touchTxt.gameObject.SetActive(false);
            titleTouch.TouchOnPanel();

            if(Managers.Game.FileLoad())
            {
                //저장파일이 있다면
                Managers.Game.GameDataInit();
                Managers.Game.FileLoad();   //파일을 불러온다
                if(Managers.Game.UnitWarriorLv == 0)        //파일을 불러왔는데 기본캐릭터인 전사가 레벨이 0이라면
                {
                    Managers.Game.GameDataInit();       //이상하게 저장된 게임파일로 초기화 시켜준다.
                    Managers.Game.SlotUnitClass.Clear();        //배열도 초기화 시켜줘야한다.
                }


                //초기화후 저장파일을 불러옴
            }
            else
            {
                //저장파일이 없다면
                Managers.Game.GameDataInit(); //초기화만

            }
            Managers.Game.UnitLvDictRefresh();


        }
    }


    T UiRaycastGetFirstComponent<T>(GraphicRaycaster gr) where T : Component
    {
        rrList.Clear();

        gr.Raycast(ped, rrList);
        if (rrList.Count == 0)
            return null;



        return rrList[0].gameObject.GetComponent<T>();
    }


    private int Description(int oldWeapon, int golds, int sellingPrice, int repairCost)
    {


        while (oldWeapon * repairCost >= golds)
        {
            oldWeapon--;
            golds += sellingPrice;
        }

        return oldWeapon;


}
}
