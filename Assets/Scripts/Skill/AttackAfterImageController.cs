using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class AttackAfterImageController : SkillBase
{

    private List<GameObject> skillEffectsObj = new List<GameObject>();
    private Coroutine startSkillEffCo = null;
    private Coroutine skillAttackHitCo = null;

    private const int FIRST_EFFECTINDEX = 2;
    private const int SECOND_EFFECTINDEX = 4;
    private const int FINAL_EFFECTINDEX = 5;

    private BoxCollider2D boxColl;

    private int hitCount = 0;

    public void SetInfo(Unit owner, SkillData data)
    {
        //if (Managers.Data.magicSkillDict.TryGetValue(Unitlv, out SkillData data) == false)
        //{
        //    Debug.LogError("ProjecteController SetInfo Failed");
        //    return;
        //}

        Owner = owner;
        SkillData = data;
        hitCount = 0;

    }



    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out boxColl);

        boxColl.enabled = false;            //�ڽ��ݶ��̴��� ó���� �����ɶ� ����ٰ� ���� (ó���� ������ ������ �۵��ؼ� �������� �����ʴ°�찡 ����)
        boxColl.enabled = true;

        skillEffectsObj.Clear();
        for(int ii = 0; ii < transform.childCount; ii++)
            skillEffectsObj.Add(transform.GetChild(ii).gameObject);     //���� ������Ʈ�� ���Ƽ� ���� ������Ʈ����Ʈ�� �־��ش�.


        startSkillEffCo = StartCoroutine(StartSkillEff());

        Destroy(gameObject, 3.0f);
    }

    WaitForSeconds wfs = new WaitForSeconds(0.3f);

    private IEnumerator StartSkillEff()
    {
        for(int ii = 0; ii < FIRST_EFFECTINDEX; ii++)
        {
            skillEffectsObj[ii].SetActive(true);
        }

        yield return wfs;

        for (int ii = FIRST_EFFECTINDEX; ii < SECOND_EFFECTINDEX; ii++)
        {
            skillEffectsObj[ii].SetActive(true);
            boxColl.enabled = false;            //�ι�° ����Ʈ�� �������뿡�� �ݶ��̴����༭ ���̻� ���������� �����ʰ� ó���� �����մ����ָ� Ÿ������
        }

        yield return wfs;

        for (int ii = SECOND_EFFECTINDEX; ii < FINAL_EFFECTINDEX; ii++)
        {
            skillEffectsObj[ii].SetActive(true);
        }

        yield return null;
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll != null && coll.tag.Contains("Unit"))
        {
            if (hitCount > 6)
                return;


            coll.TryGetComponent(out Unit unit);

            StartCoroutine(SkillAttackHit(unit));
            hitCount++;
            
        }



        
    }

    WaitForSeconds skillWfs = new WaitForSeconds(0.1f);     //0.1�ʰ��� ������

    private IEnumerator SkillAttackHit(Unit unit)
    {

        for (int jj = 0; jj < SkillData.skillHitCount; jj++)
        {
            if (Owner.CriticalCheck())
                unit.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)) * 2, 0, true);      //�˹��� ���� ũ��Ƽ���� ����

            
            else
                unit.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)));      //��ũ��Ƽ��

            

            yield return skillWfs;
        }



        yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
