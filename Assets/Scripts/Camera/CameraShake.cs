using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera monGateVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera towerVirtualCamera;

    Camera mainCamera;

    Vector3 cameraPos;

    private float shakeTime;
    private float shakeIntensity;

    private void Start()
    {
        cameraPos = Camera.main.transform.position;
    }

    public void VirtulCameraShake(float amplitudeGainValue, float frequencyGainValue, bool isTower = false)
    {
        //if (mainCamera == null)
        //    TryGetComponent(out mainCamera);

        //cameraPos = mainCamera.transform.position;
        ////Test();
        //StartCoroutine(StartShake());
        ////InvokeRepeating("StartShake", 0.0f, 0.005f);
        ////Invoke("StopShake",duration);
        //StartCoroutine(StopShake(duration));
        CinemachineBasicMultiChannelPerlin perlin = null;
        if (!isTower)
            perlin = monGateVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        else
            perlin = towerVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();



        perlin.m_AmplitudeGain = amplitudeGainValue;
        perlin.m_FrequencyGain = frequencyGainValue;


    }

    //public void OnShakeCamera(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    //{
        
    //    this.shakeTime = shakeTime;
    //    this.shakeIntensity = shakeIntensity;

    //    StopCoroutine("ShakeByPosition");
    //    StartCoroutine("ShakeByPosition");
    //}

    //private IEnumerator ShakeByPosition()
    //{
    //    cameraPos = Camera.main.transform.position;
    //    Vector3 startPos = transform.position;

    //    while( shakeTime > 0.0f)
    //    {
    //        transform.position = startPos + Random.insideUnitSphere * shakeIntensity;

    //        shakeTime -= Time.deltaTime;

    //        yield return null;
    //    }

    //    transform.position = cameraPos;
    //}


  





}
