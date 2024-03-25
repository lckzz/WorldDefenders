using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class GPGS_Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logTxt;
    // Start is called before the first frame update
    void Start()
    {
        GPGS_LogIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GPGS_LogIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
            string userID = PlayGamesPlatform.Instance.GetUserId();
            logTxt.text = "로그인 성공 : " + displayName + " / " + userID;
        }
        else
        {
            logTxt.text = "로그인 실패";

        }
    }
}
