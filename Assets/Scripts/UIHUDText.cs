using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHUDText : MonoBehaviour
{
    [SerializeField] private float moveDist = 10;
    [SerializeField] private float moveTime = 1.5f;

    [SerializeField] private RectTransform rt;
    [SerializeField] private TextMeshProUGUI textHUD;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out rt);
        TryGetComponent(out textHUD);
    }

    public void Play(string text, Color color, Vector3 pos, float gap = 0.1f)
    {
        if(textHUD == null)
        {
            TryGetComponent(out rt);
            TryGetComponent(out textHUD);
        }

        textHUD.text = text;
        textHUD.color = color;
        StartCoroutine(OnHUDText(pos, gap));
    }


    private IEnumerator OnHUDText(Vector3 pos, float gap)
    {
        Vector3 start = new Vector3(pos.x,pos.y + 1.0f,0.0f);
        Vector3 end = start + Vector3.up * moveDist;

        float cur = 0;
        float percent = 0;

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
