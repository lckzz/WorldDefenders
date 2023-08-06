using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitUpgradePopUp : MonoBehaviour
{

    public Button closeBtn;


    // Start is called before the first frame update
    void Start()
    {
        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                if (this.gameObject.activeSelf)
                    gameObject.SetActive(false);
            });
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenUpgradePopUpInit()
    {
        //해당되는 클래스의 정보를 받아와서 그에 맞는 이미지로 변환
        //해당 클래스의 json형식을 받아와서 적용
    }
}
