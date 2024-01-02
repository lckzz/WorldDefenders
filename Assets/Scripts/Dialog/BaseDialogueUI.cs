using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseDialogueUI : MonoBehaviour
{
    [SerializeField] protected GameObject dialogPanelObj;
    [SerializeField] protected TextMeshProUGUI dialogtxt;
    [SerializeField] protected GameObject dialogArrow;


    protected DialogueCtrl dialogCtrl;


    // Start is called before the first frame update

    public TextMeshProUGUI DialogTxt { get { return dialogtxt; } }

    protected virtual void Start()
    {
        GameObject parentGo = this.transform.parent.gameObject;

        parentGo.TryGetComponent(out dialogCtrl);


    }


    public void ShowDialogPanel()
    {
        dialogPanelObj.SetActive(true);
        
    }

    public void HideDialogPanel()
    {
        dialogPanelObj.SetActive(false);
    }

    public void ShowDialogArrow()
    {
        dialogArrow.SetActive(true);
    }

    public void HideDialogArrow()
    {
        dialogArrow.SetActive(false);
    }





    public bool DialogArrowIsActive()
    {
        if (dialogArrow.activeSelf == true)
            return true;
        else
            return false;
    }
}
