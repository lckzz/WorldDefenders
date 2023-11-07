using DG.Tweening;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueCtrl : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanelObj;
    [SerializeField] private TextMeshProUGUI dialogtxt;

    private readonly string tutorialKey = "tutorial";
    private JSONNode node;

    private bool click = false;
    private float typingSpeed = 25f;

    private void Start()
    {
        node = Managers.Dialog.DialogJsonParsing(tutorialKey);
        Managers.Dialog.DialogSetting(tutorialKey, node);

        StartCoroutine(StartDialog());
    }

    void Update()
    {

        if(Input.GetMouseButtonUp(0))
        {
            click = true;
        }
    }

    IEnumerator StartDialog()
    {
        for (int ii = 0; ii < node[tutorialKey]["count"]; ii++)
        {
            yield return StartCoroutine(ShowDialog());
        }

        dialogPanelObj.SetActive(false);


        IEnumerator ShowDialog()
        {
            dialogPanelObj.SetActive(true);
            dialogtxt.text = "";

            float timePerChar = 1 / typingSpeed;
            string text = Managers.Dialog.DialogQueue.Dequeue();

            float duration = text.Length * timePerChar;

            Debug.Log(text.Length);
            dialogtxt.DOText(text, duration).SetEase(Ease.Linear);
           
            
            while (true)
            {
                if (click)
                {
                    click = false;
                    yield break;
                }

                yield return null;
            }
        }


    }
}
