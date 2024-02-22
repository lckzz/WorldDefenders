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

        boxColl.enabled = false;            //박스콜라이더를 처음에 생생될때 꺼줬다가 켜줌 (처음에 동작이 느리게 작동해서 데미지를 입지않는경우가 있음)
        boxColl.enabled = true;

        skillEffectsObj.Clear();
        for(int ii = 0; ii < transform.childCount; ii++)
            skillEffectsObj.Add(transform.GetChild(ii).gameObject);     //하위 오브젝트를 돌아서 전부 오브젝트리스트에 넣어준다.


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
            boxColl.enabled = false;            //두번째 이펙트가 켜질때쯤에는 콜라이더꺼줘서 더이상 들어오는적은 맞지않고 처음에 속해잇던유닛만 타격입음
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

    WaitForSeconds skillWfs = new WaitForSeconds(0.1f);     //0.1초간의 딜레이

    private IEnumerator SkillAttackHit(Unit unit)
    {

        for (int jj = 0; jj < SkillData.skillHitCount; jj++)
        {
            if (Owner.CriticalCheck())
                unit.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)) * 2, 0, true);      //넉백은 없고 크리티컬은 터짐

            
            else
                unit.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)));      //노크리티컬

            

            yield return skillWfs;
        }



        yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
