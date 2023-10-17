using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingTitleTxt;
    [SerializeField] private Image prograssBar;
    [SerializeField] private TextMeshProUGUI loadingTxt;
    [SerializeField] private TextMeshProUGUI loadingTipTxt;

    [SerializeField] private GameObject fieldLoadingBg;
    [SerializeField] private GameObject lobbyLoadingBg;




    private Define.Scene nextScene;
    private Define.SubStage subStage;
    private List<string> loadingStrs = new List<string>();

    private readonly string loadingStr1 = "Loading.";
    private readonly string loadingStr2 = "Loading..";
    private readonly string loadingStr3 = "Loading...";
    private int loadingStrCount = 3;

    private readonly string lobbyStageStr = "유닛을 업그레이드하고 원하는 유닛을 배치하세요!.";
    private readonly string westStageStr = "서부 숲 지대에서는 강력한 전사가 등장한다고 합니다.";
    private readonly string eastStageStr = "동부 숲 지대의 몬스터중의 하나는 적들을 끌어올 수 있다고 합니다.";
    private readonly string southStageStr = "몬스터를 이끄는 기마병이 존재 한다고 합니다.";



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Managers.Loading.LoadScene(prograssBar));

        LoadingTextInit();


    }

    IEnumerator LoadingTxtUpdate()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.3f);
        int count = 0;

        while(true)
        {
            if (count >= 3)
                count = 0;

            yield return wfs;

            loadingTxt.text = loadingStrs[count];
            count++;

            yield return null;
        }
    }


    private void LoadingTextInit()
    {
        loadingStrs.Clear();
        NextSceneSetting();
        for (int ii = 0; ii < loadingStrCount; ii++)
        {
            switch (ii)
            {
                case 0:
                    loadingStrs.Add(loadingStr1);
                    break;
                case 1:
                    loadingStrs.Add(loadingStr2);
                    break;
                case 2:
                    loadingStrs.Add(loadingStr3);
                    break;
            }

        }


        StartCoroutine(LoadingTxtUpdate());

    }


    private void NextSceneSetting()
    {
        subStage = Managers.Game.CurStageType;
        nextScene = Managers.Loading.NextScene;
        switch (nextScene)
        {
            case Define.Scene.Lobby:
                loadingTitleTxt.text = "로비";
                loadingTipTxt.text = lobbyStageStr;
                lobbyLoadingBg.SetActive(true);
                break;

            case Define.Scene.BattleStage_Field:
                fieldLoadingBg.SetActive(true);
                CurBattleStageSet();
                break;
        }

    }


    private void CurBattleStageSet()
    {
        switch (subStage)
        {
            case Define.SubStage.West:
                loadingTitleTxt.text = "서부 숲 지대";
                loadingTipTxt.text = westStageStr;

                break;
            case Define.SubStage.East:
                loadingTitleTxt.text = "동부 숲 지대";
                loadingTipTxt.text = eastStageStr;

                break;
            case Define.SubStage.South:
                loadingTitleTxt.text = "남부 숲 지대";
                loadingTipTxt.text = southStageStr;


                break;

        }

    }
}
