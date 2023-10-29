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

    private Define.SubStage subStage;


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
        switch (subStage)
        {
            case Define.SubStage.West:
                stageNameTxt.text = "¼­ºÎ ½£ Áö´ë";

                break;
            case Define.SubStage.East:
                stageNameTxt.text = "µ¿ºÎ ½£ Áö´ë";

                break;
            case Define.SubStage.South:
                stageNameTxt.text = "³²ºÎ ½£ Áö´ë";
                break;

        }
    }



    WaitForSeconds wfs = new WaitForSeconds(1.0f);
    IEnumerator StartFadeTxt()
    {
        yield return wfs;
        stageNameTxt.DOFade(0, 2.0f);

    }

}
