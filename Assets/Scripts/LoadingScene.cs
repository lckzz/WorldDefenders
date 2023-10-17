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

    private readonly string lobbyStageStr = "������ ���׷��̵��ϰ� ���ϴ� ������ ��ġ�ϼ���!.";
    private readonly string westStageStr = "���� �� ���뿡���� ������ ���簡 �����Ѵٰ� �մϴ�.";
    private readonly string eastStageStr = "���� �� ������ �������� �ϳ��� ������ ����� �� �ִٰ� �մϴ�.";
    private readonly string southStageStr = "���͸� �̲��� �⸶���� ���� �Ѵٰ� �մϴ�.";



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
                loadingTitleTxt.text = "�κ�";
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
                loadingTitleTxt.text = "���� �� ����";
                loadingTipTxt.text = westStageStr;

                break;
            case Define.SubStage.East:
                loadingTitleTxt.text = "���� �� ����";
                loadingTipTxt.text = eastStageStr;

                break;
            case Define.SubStage.South:
                loadingTitleTxt.text = "���� �� ����";
                loadingTipTxt.text = southStageStr;


                break;

        }

    }
}
