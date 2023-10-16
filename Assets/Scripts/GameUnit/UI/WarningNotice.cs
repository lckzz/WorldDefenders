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

    private readonly string warningWave = "����Ʈ���� ���Ͱ� �뷮���� �����ɴϴ�!..";
    private readonly string warningElite = "����Ʈ���� ������ ���� ��ü ����!";
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
        //Debug.Log("���� �׼��Լ��� ��������");
        if (Managers.Game.EliteMonsterCheck() || Managers.Game.FinalMonsterCheck())               //����Ʈ ���� �̺�Ʈ�� �߻��ϸ�
        {

            //���â�� üũ�� ����������
            if (co != null)
                StopCoroutine(co);
            co = StartCoroutine(WarningObject(warningElite)); //���â�� ���ش�


            
        }

        else if (Managers.Game.MonsterWaveCheck())          // ���̺��̺�Ʈ�� �߻��ϸ�
        {

            if (co != null)
                StopCoroutine(co);

            co = StartCoroutine(WarningObject(warningWave));  //���â�� ���ش�
            
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
        Debug.Log("��Ʈ�� ���⼭ ����������");
        yield return wfs;
        Vector3 pos = rt.localPosition;
        pos.x = 1300.0f;
        rt.localPosition = pos;



    }


}
