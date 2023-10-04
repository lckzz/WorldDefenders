using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCharactorSetting : MonoBehaviour
{
    [SerializeField] GameObject[] loadingCharctors;
    [SerializeField] GameObject canvasObj;
    private Animator anim;
    private RectTransform rt;
    int randIdx = 0;
    // Start is called before the first frame update
    void Awake()
    {
        randIdx = UnityEngine.Random.Range(0, loadingCharctors.Length);
        GameObject go = Managers.Resource.Instantiate(loadingCharctors[randIdx],canvasObj.transform);
        go.TryGetComponent(out anim);
        go.TryGetComponent(out rt);

        anim.SetBool("Run", true);
        rt.localScale = new Vector3(-1, 1, 1);
    }


}
