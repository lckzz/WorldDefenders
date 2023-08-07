using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{

    //로비를 전체적으로 관리하고 보여주는 스크립트

    // Start is called before the first frame update
    void Start()
    {
        //Managers.UI.ShowSceneUI<UI_Lobby>();
        Managers.UI.ShowPopUp<UI_Lobby>();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
