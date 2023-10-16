using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class WarningNotice : MonoBehaviour
{

    [SerializeField] private GameObject warningObj;
    private RectTransform rt;
    private TextMeshProUGUI text;
    private bool warningCheck = false;

    private Coroutine co;



    // Start is called before the first frame update
    private void Start()
    {
        warningObj.TryGetComponent(out rt);
        warningObj.transform.GetChild(0).TryGetComponent(out text);

    }


    // Update is called once per frame
    void Update()
    {

        //Debug.Log(Managers.Game.EliteMonsterCheck());
        //Debug.Log(warningCheck);


        //WarningObjisOn();
    }


    public void WarningObjisOn(string warningTxt)
    {

        //경고창의 체크가 꺼져있을때
        if (co != null)
            StopCoroutine(co);
        co = StartCoroutine(WarningObject(warningTxt)); //경고창을 켜준다


        //if (Managers.Game.MonsterWaveCheck())
        //{
        //    if (warningCheck == false)
        //    {
        //        if (Managers.Game.EliteMonsterCheck())
        //        {
        //            StartCoroutine(WarningObject(warningElite));
        //        }
        //        else
        //            StartCoroutine(WarningObject(warningWave));

        //    }
        //}
        //else if(Managers.Game.MonsterNormalCheck())
        //{
        //    if (warningCheck == false)
        //    {
        //        if (Managers.Game.EliteMonsterCheck())
        //        {
        //            StartCoroutine(WarningObject(warningElite));
        //        }


        //    }
        //}
        //else
        //    warningCheck = false;
    }

    IEnumerator WarningObject(string warningTxt)
    {
        rt.localPosition = new Vector3(1300.0f, rt.localPosition.y, rt.localPosition.z);
        rt.DOKill();

        Define.MonsterSpawnType type = Managers.Game.GetMonSpawnType();
        Managers.Game.SetMonSpawnType(type);
        //Debug.Log(Managers.Game.EliteMonsterCheck());

        WaitForSeconds wfs = new WaitForSeconds(5.0f);

        if (warningObj.activeSelf == false)
            warningObj.SetActive(true);

        text.text = warningTxt;

        rt.DOLocalMoveX(0.0f, 1.0f).SetEase(Ease.OutBounce);
        yield return wfs;
        rt.DOLocalMoveX(-1300.0f, 1.0f).SetEase(Ease.OutBack);
        Debug.Log("두트윈 여기서 ㄴ움직여요");
        yield return wfs;
        Vector3 pos = rt.localPosition;
        pos.x = 1300.0f;
        rt.localPosition = pos;



    }


}
