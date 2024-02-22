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

    private readonly string stagetip1 = "������ ���׷��̵��ϰ� ���ϴ� ������ ��ġ�ϼ���!.";
    private readonly string stagetip2 = "����������� �����ð����� ��ų�� �ڵ����� ����� �� �ֽ��ϴ�.";
    private readonly string stagetip3 = "���� �� ���뿡���� ������ ���簡 �����Ѵٰ� �մϴ�.";

    private readonly string stagetip4 = "���� �� ������ �������� �ϳ��� ������ ����� �� �ִٰ� �մϴ�.";
    private readonly string stagetip5 = "������ ���׷��̵��ϸ� ���� �������� ��ų�� ����˴ϴ�";

    private readonly string stagetip6 = "���͸� �̲��� �⸶���� ���� �Ѵٰ� �մϴ�.";
    private readonly string stagetip7 = "���迡�� ���� ����Ʈ�� ����ؼ� �����ǰ� �ִٰ� �մϴ�.";

    private readonly string stagetip8 = "�Ϻο��� �������� ����Ʈ����� ���� �� ��� �������ϴ�.";
    private readonly string stagetip9 = "���̷����� ���� ������ ���͵��� ��ȯ�� �� �ִٰ� �մϴ�.";

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
                loadingTitleTxt.text = "�κ�";
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
                loadingTitleTxt.text = "���� �� ����";

                break;
            case Define.Stage.South:
                loadingTitleTxt.text = "���� �� ����";

                break;
            case Define.Stage.East:
                loadingTitleTxt.text = "���� �� ����";

                break;
            case Define.Stage.Boss:
                loadingTitleTxt.text = "�Ϻ� �� ����";


                break;

        }

    }
}
