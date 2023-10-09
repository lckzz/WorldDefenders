using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDust : MonoBehaviour
{

    [SerializeField] private Animator towerDust1;
    [SerializeField] private Animator towerDust2;
    [SerializeField] private Animator towerDust3;

    private bool animatorCheck = false;
    private float towerDustTime1 = .0f;
    private float towerDustTime2 = .0f;
    private float towerDustTime3 = .0f;

    private WaitForSeconds wfs = new WaitForSeconds(0.4f);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartOpenGateDust());
    }

    private void Update()
    {
        if (animatorCheck)
        {
            TowerDustEnd(towerDustTime1, towerDust1);
            TowerDustEnd(towerDustTime2, towerDust2);
            TowerDustEnd(towerDustTime3, towerDust3);

        }
    }

    private void TowerDustEnd(float time, Animator anim)
    {
        time = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        time -= (int)anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (time >= 0.9f)
        {
            anim.speed = 0;
            anim.gameObject.SetActive(false);

        }
    }


    IEnumerator StartOpenGateDust()
    {
        if (towerDust1 == null || towerDust2 == null || towerDust3 == null)
            yield break;

        towerDust1.gameObject.SetActive(true);
        yield return wfs;

        towerDust2.gameObject.SetActive(true);

        yield return wfs;
        towerDust3.gameObject.SetActive(true);


        yield return null;
    }

    public void TowerDustDisEnable()
    {
        animatorCheck = true;
    }
}
