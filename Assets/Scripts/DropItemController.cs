using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropItemController : MonoBehaviour
{

    Vector3 curPos = Vector3.zero;
    float yPos = .0f;
    // Start is called before the first frame update
    void Start()
    {
        //transform.DOLocalMoveY(2.5f, 0.75f).OnComplete(() =>
        //{
        //    transform.DOLocalMoveY(0.0f, 0.75f).SetEase(Ease.InSine);
        //}).SetEase(Ease.OutSine);
        transform.DOLocalRotate(new Vector3(0, 0, 2160.0f), 1.45f,RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(()=>
        {
            //StartCoroutine(SinDropItemMove());
        });
    }


    IEnumerator SinDropItemMove()
    {
        while(true)
        {
            curPos = this.transform.position;

            yPos += Time.deltaTime * 1.5f;


            curPos.y = Mathf.Sin(yPos) * 0.15f;

            transform.position = curPos;

            yield return null;
        }
    }

}
