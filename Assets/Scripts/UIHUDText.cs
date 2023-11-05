using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class UIHUDText : MonoBehaviour
{
    [SerializeField] private float moveDist = 10;
    [SerializeField] private float moveTime = 1.5f;


    [SerializeField] private RectTransform rt;
    [SerializeField] private TextMeshProUGUI textHUD;
    [SerializeField] private Image itemImg;
    [SerializeField] private Sprite[] itemSprite;


    private float scalePunchValue = 0.025f;
    private float scalePunchDuration = 0.5f;
    private int scalePunchVibrato = 1;
    private Vector3 rtScalePunch = Vector3.zero;

    private int itemIdx;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out rt);
        TryGetComponent(out textHUD);
    }

    public void Play(string text, Color color, Vector3 pos,bool critical = false, bool item = false)
    {

        if (textHUD == null)
        {
            TryGetComponent(out rt);
            TryGetComponent(out textHUD);
            rtScalePunch = new Vector3(scalePunchValue, scalePunchValue, rt.localScale.z);

        }

        if(item)
        {
            Debug.Log("여기 아이템 " + itemIdx);
            itemImg.gameObject.SetActive(true);
            itemImg.sprite = itemSprite[itemIdx];


            textHUD.text = $"+ {text}";
        }
        else
            textHUD.text = text;


        textHUD.color = color;

        StartCoroutine(OnHUDText(pos, critical));
    }

    public void ItemIdx(int idx)
    {
        itemIdx = idx;
    }


    private IEnumerator OnHUDText(Vector3 pos,bool critical)
    {
        Vector3 start = new Vector3(pos.x,pos.y + 1.0f,0.0f);
        Vector3 end = start + Vector3.up * moveDist;

        float cur = 0;
        float percent = 0;

        if (critical)
            rt.DOPunchScale(rtScalePunch, scalePunchDuration, scalePunchVibrato);


        while(percent < 1)
        {
            cur += Time.deltaTime;
            percent = cur / moveTime; 


            rt.position = Vector2.Lerp(start,end, percent);

            Color color = textHUD.color;
            color.a = Mathf.Lerp(1, 0, percent);
            textHUD.color = color;

            yield return null;
        }

        Managers.Resource.Destroy(gameObject);
    }


}
