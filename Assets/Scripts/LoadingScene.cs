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
    private Define.Stage subStage;
    private List<string> loadingStrs = new List<string>();
    private List<string> loadingTipsStrs = new List<string>();
    private int tipCnt = 10;

    private readonly string loadingStr1 = "Loading.";
    private readonly string loadingStr2 = "Loading..";
    private readonly string loadingStr3 = "Loading...";
    private int loadingStrCount = 3;

    private readonly string stagetip1 = "유닛을 업그레이드하고 원하는 유닛을 배치하세요!.";
    private readonly string stagetip2 = "스페셜유닛은 일정시간마다 스킬을 자동으로 사용할 수 있습니다.";
    private readonly string stagetip3 = "서부 숲 지대에서는 강력한 전사가 등장한다고 합니다.";

    private readonly string stagetip4 = "동부 숲 지대의 몬스터중의 하나는 적들을 끌어올 수 있다고 합니다.";
    private readonly string stagetip5 = "리더를 업그레이드하면 일정 레벨마다 스킬이 개방됩니다";

    private readonly string stagetip6 = "몬스터를 이끄는 기마병이 존재 한다고 합니다.";
    private readonly string stagetip7 = "세계에는 지금 게이트가 계속해서 생성되고 있다고 합니다.";

    private readonly string stagetip8 = "북부에서 느껴지는 게이트기운은 더욱 더 깊게 느껴집니다.";
    private readonly string stagetip9 = "스켈레톤의 왕은 강력한 몬스터들을 소환할 수 있다고 합니다.";

    private Dictionary<int, string> tipDict;




    // Start is called before the first frame update
    void Start()
    {
        tipDict = new Dictionary<int, string>
        {
            {1,stagetip1 },
            {2,stagetip2 },
            {3,stagetip3 },
            {4,stagetip4 },
            {5,stagetip5 },
            {6,stagetip6 },
            {7,stagetip7 },
            {8,stagetip8 },
            {9,stagetip9 }

        };
        loadingTipsStrs.Clear();
        for (int ii = 1; ii < tipCnt; ii++)
        {
            loadingTipsStrs.Add(tipDict[ii]);
        }

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
        int rnd = Random.Range(0, 9);
        switch (nextScene)
        {
            case Define.Scene.Lobby:
                loadingTitleTxt.text = "로비";
                loadingTipTxt.text = loadingTipsStrs[rnd];
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
        int rnd = Random.Range(0, 9);
        loadingTipTxt.text = loadingTipsStrs[rnd];

        switch (subStage)
        {
            case Define.Stage.West:
                loadingTitleTxt.text = "서부 숲 지대";

                break;
            case Define.Stage.South:
                loadingTitleTxt.text = "남부 숲 지대";

                break;
            case Define.Stage.East:
                loadingTitleTxt.text = "동부 숲 지대";

                break;
            case Define.Stage.Boss:
                loadingTitleTxt.text = "북부 숲 지대";


                break;

        }

    }
}
