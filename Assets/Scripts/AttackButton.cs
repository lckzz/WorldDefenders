using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    private PlayerController player;
    private Image coolImg;
    private float speed = 1.0f;

    private void Start()
    {
        GameObject.Find("Player").TryGetComponent<PlayerController>(out player);
        transform.Find("Cool").gameObject.TryGetComponent<Image>(out coolImg);
    }

    private void Update()
    {
        UpdateCoolTime();
    }

    void UpdateCoolTime()
    {
        float coolTime = player.PercentCoolTime();

        if (coolImg.fillAmount > coolTime)
            coolImg.fillAmount -= Time.deltaTime * speed;

        if (coolTime >= 1.0f)
            coolImg.fillAmount = 1;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        player.AttackWait();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.ShotArrow();
    }
}
