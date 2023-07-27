using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class UI_GameResult : MonoBehaviour
{

    [SerializeField]
    private Image failOrVictoryImg;
    [SerializeField]
    private Button retryBtn;
    [SerializeField]
    private Button exitBtn;
    [SerializeField]
    private TextMeshProUGUI timer;


    bool timecheck = false;
    int min = 0;
    int sec = 0;
    int curmin = 0;
    int cursec = 0;

    WaitForSeconds timewfs = new WaitForSeconds(0.05f);
    WaitForSeconds wfs = new WaitForSeconds(0.5f);

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(timecheck)
        {
            timecheck = false;
            StartCoroutine(Timer());
        }


    }

    private void OnEnable()
    {
        if(GameManager.instance.state == GameState.GameVictory)
        {
            failOrVictoryImg.sprite = ImageSetting("Images/Image_Victory");
        }

        if(GameManager.instance.state == GameState.GameFail)
        {
            failOrVictoryImg.sprite = ImageSetting("Images/Image_Fail");

        }

        TimerSetting();
    }



    void TimerSetting()
    {
        min = (int)GameManager.instance.timerSec / 60;
        sec = (int)GameManager.instance.timerSec % 60;
        timecheck = true;
    }


    private void timeSet(int min, int sec)
    {
        if (min < 10 && sec < 10)
            timer.text =  "0" + min.ToString() + " : " + "0" + sec.ToString();

        else if (min >= 10 && sec < 10)       //초만 10초 미만일때
            timer.text = min.ToString() + " : " + "0" + sec.ToString();

        else if (min < 10 && sec >= 10)       //분만 10초 미만일때
            timer.text = "0" + min.ToString() + " : " + sec.ToString();

        else if ( min < 10 && sec < 10)       //초 분이 10 미만일때
            timer.text = "0" + min.ToString() + " : " + "0" + sec.ToString();

        else if (min >= 10 && sec < 10)       //초가 10 미만일때
            timer.text =  min.ToString() + " : " + "0" + sec.ToString();

        else if (min < 10 && sec >= 10)       //분가 10 미만일때
            timer.text = "0" + min.ToString() + " : " + sec.ToString();

        else   //전부다 10 이상일때
            timer.text = min.ToString() + " : " + sec.ToString();
    }

    IEnumerator Timer()
    {

        yield return wfs;

        bool timer = true;
        while(timer)
        {

            if (curmin < min)
                curmin++;

            if (cursec < sec)
                cursec++;

            timeSet(curmin, cursec);
            

            int curTimer = (curmin * 60) + cursec;

            if (curTimer == GameManager.instance.timerSec)
                timer = false;


            yield return timewfs;
        }
    }


    Sprite ImageSetting(string path)
    {
        return Resources.Load<Sprite>(path);
    }





}
