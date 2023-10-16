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

    private readonly string warningWave = "게이트에서 몬스터가 대량으로 몰려옵니다!..";
    private readonly string warningElite = "게이트에서 강력한 몬스터 개체 출현!";
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


    public void WarningObjisOn()
    {
        //Debug.Log("여기 액션함수로 들어왓져요");
        if (Managers.Game.EliteMonsterCheck() || Managers.Game.FinalMonsterCheck())               //엘리트 몬스터 이벤트가 발생하면
        {

            //경고창의 체크가 꺼져있을때
            if (co != null)
                StopCoroutine(co);
            co = StartCoroutine(WarningObject(warningElite)); //경고창을 켜준다


            
        }

        else if (Managers.Game.MonsterWaveCheck())          // 웨이브이벤트가 발생하면
        {

            if (co != null)
                StopCoroutine(co);

            co = StartCoroutine(WarningObject(warningWave));  //경고창을 켜준다
            
        }



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
