using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StageStart : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startTxt;
    [SerializeField] private TextMeshProUGUI stageNameTxt;
    [SerializeField] private Image startUiImg;
    [SerializeField] private GameObject ui_GamePlay;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("wewewe");

    }


    private void OnEnable()
    {
        StartCoroutine(StartUI());
    }


    WaitForSeconds startOnWfs = new WaitForSeconds(0.5f);
    WaitForSeconds startOffWfs = new WaitForSeconds(0.8f);
    WaitForSeconds startWfs = new WaitForSeconds(0.7f);


    IEnumerator StartUI()
    {
        stageNameTxt.DOFade(1, 0.5f);
        startUiImg.DOFade(0.9f, 0.5f);

        yield return startOnWfs; //초동안 대기하고
        startTxt.transform.DOLocalMoveX(0.0f, 0.7f).SetEase(Ease.OutQuad);
        startTxt.DOFade(1, 0.7f);
        yield return startOffWfs; //초동안 대기하고

        startTxt.transform.DOLocalMoveX(300.0f, 0.7f).SetEase(Ease.OutQuad);
        startTxt.DOFade(0, 0.7f);

        yield return startWfs; //초동안 대기하고
        stageNameTxt.DOFade(0, 0.5f);
        startUiImg.DOFade(0f, 0.5f);

        yield return startOnWfs; //초동안 대기하고
        stageNameTxt.gameObject.SetActive(false);
        ui_GamePlay?.SetActive(true);


    }

}
