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
}
