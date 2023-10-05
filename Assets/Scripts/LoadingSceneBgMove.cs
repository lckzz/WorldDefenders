using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneBgMove : MonoBehaviour
{
    [SerializeField] private float endvalue = -0.0f;
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private float posY = 0.0f;

    RectTransform rt;
    Vector2 vec = Vector2.zero;
    Vector2 resetVec = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        vec = new Vector2(-speed * Time.deltaTime, 0.0f);
        resetVec = new Vector2(0.0f, posY);
        gameObject.TryGetComponent(out rt);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(speed);
        rt.anchoredPosition += vec;
        if(rt.anchoredPosition.x <= endvalue)
            rt.anchoredPosition = resetVec;
        //this.transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
