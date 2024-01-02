using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour,IPointerClickHandler
{



    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> rrList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, rrList);

        foreach(RaycastResult result in rrList)
        {
            result.gameObject.TryGetComponent(out Button button);
            if(button != null)
            {
                Debug.Log("¹öÆ°" + button.name);

                break;
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame

}
