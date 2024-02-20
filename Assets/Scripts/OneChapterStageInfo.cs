using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OneChapterStageInfo 
{
    [SerializeField]
    StageData stageData;

    public StageData StageData { get { return stageData; } set { stageData = value; } }

    public int id;
    public int state;
    public float bestTime;
    public bool clear;

    public OneChapterStageInfo(StageData data, int id, bool clear = false, int state = 0)
    {
        this.id = id;
        this.stageData = data;
        this.state = state;
        this.bestTime = 0;
        this.clear = clear;
    }


}
