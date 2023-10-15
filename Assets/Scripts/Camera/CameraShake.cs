using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 cameraPos;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    Camera mainCamera;
    [SerializeField]
    [Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    [SerializeField]
    [Range(0.1f, 1f)] float duration = 0.5f;


    public void VirtulCameraShake(float amplitudeGainValue, float frequencyGainValue)
    {
        //if (mainCamera == null)
        //    TryGetComponent(out mainCamera);

        //cameraPos = mainCamera.transform.position;
        ////Test();
        //StartCoroutine(StartShake());
        ////InvokeRepeating("StartShake", 0.0f, 0.005f);
        ////Invoke("StopShake",duration);
        //StartCoroutine(StopShake(duration));

        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = amplitudeGainValue;
        perlin.m_FrequencyGain = frequencyGainValue;

    }



    IEnumerator StartShake()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.005f);

        while (true)
        {
            cameraPos = mainCamera.transform.position;

            float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
            float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
            cameraPos = mainCamera.transform.position;
            cameraPos.x += cameraPosX;
            cameraPos.y += cameraPosY;
            mainCamera.transform.position = cameraPos;
            Debug.Log(mainCamera.transform.position);
            yield return wfs;
        }
    }



    //void StartShake()
    //{
    //    Debug.Log("여기 흔들고있어요");
    //    float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
    //    float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
    //    cameraPos = Camera.main.transform.position;
    //    cameraPos.x += cameraPosX;
    //    cameraPos.y += cameraPosY;
    //    Camera.main.transform.position = cameraPos;
    //}

    IEnumerator StopShake(float duration)
    {

        WaitForSeconds wfs = new WaitForSeconds(duration);
        yield return wfs;
        Debug.Log("wqewqe");


        StopCoroutine(StartShake());
        Camera.main.transform.position = cameraPos;
    }

    void StopShake()
    {
        CancelInvoke("StartShake");
        mainCamera.transform.position = cameraPos;
    }


}
