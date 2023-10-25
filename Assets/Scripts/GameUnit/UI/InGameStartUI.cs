using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class InGameStartUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button startTimeLineSkipBtn;
    [SerializeField] private GameObject gameSceneGo;
    [SerializeField] private PlayableDirector playable;
    void Start()
    {
        gameSceneGo = GameObject.Find("GameScene");
        gameSceneGo.TryGetComponent(out playable);


        if (startTimeLineSkipBtn != null)
            startTimeLineSkipBtn.onClick.AddListener(() =>
            {
                playable.time = playable.duration;
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
