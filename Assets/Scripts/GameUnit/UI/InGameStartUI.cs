using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class InGameStartUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button startTimeLineSkipBtn;
    [SerializeField] private GameObject gameSceneGo;
    [SerializeField] private PlayableDirector playable;
    [SerializeField] private TextMeshProUGUI stageNameTxt;

    private Define.Stage subStage;


    void Start()
    {
        gameSceneGo = GameObject.Find("GameScene");
        gameSceneGo.TryGetComponent(out playable);
        StageTextInit();
        StartCoroutine(StartFadeTxt());

        if (startTimeLineSkipBtn != null)
            startTimeLineSkipBtn.onClick.AddListener(() =>
            {
                playable.time = playable.duration;
            });

    }

    private void StageTextInit()
    {
        subStage = Managers.Game.CurStageType;
        stageNameTxt.text = Managers.Game.OneChapterStageInfoList[(int)subStage].StageData.name;

    }



    WaitForSeconds wfs = new WaitForSeconds(1.0f);
    IEnumerator StartFadeTxt()
    {
        yield return wfs;
        stageNameTxt.DOFade(0, 2.0f);

    }

}
