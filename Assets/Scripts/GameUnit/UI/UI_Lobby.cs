using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    public Button upgradeBtn;
    public GameObject upgradeObj;
    // Start is called before the first frame update
    void Start()
    {
        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeOn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpgradeOn()
    {
        this.gameObject.SetActive(false);
        if (upgradeObj != null)
            upgradeObj.SetActive(true);
    }
}
