using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TutorialMaskCtrl : MonoBehaviour
{

    private int maskObjCnt = 3;
    [SerializeField] private GameObject[] maskObjs;


    // Start is called before the first frame update
    void Start()
    {
        maskObjs = new GameObject[maskObjCnt];

        for(int ii = 0; ii < maskObjCnt; ii++)
        {
            maskObjs[ii] = gameObject.transform.GetChild(ii).gameObject;
        }
    }


    public void ShowMaskObject(int idx)
    {
        for(int ii = 0; ii < maskObjs.Length; ii++)
        {
            if (ii == idx)
                maskObjs[idx].SetActive(true);
            else
                maskObjs[ii].SetActive(false);
        }
    }

    public void HideAllMaskObject()
    {
        for(int ii = 0; ii < maskObjs.Length; ii++)
        {
            maskObjs[ii].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
