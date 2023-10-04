using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Image prograssBar;
    [SerializeField] private TextMeshProUGUI loadingTxt;

    private List<string> loadingStrs = new List<string>();

    private string loadingStr1 = "Loading.";
    private string loadingStr2 = "Loading..";
    private string loadingStr3 = "Loading...";
    private int loadingStrCount = 3;


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
}
