using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField] CinemachineVirtualCamera vCamera;
    CinemachineBasicMultiChannelPerlin noise;
    float shakeTimer;
    float shakeTimerTotal;
    float startIntensity;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        noise = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {
        EventManager.OnGameOver += HandleOnGameOver;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= HandleOnGameOver;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            Debug.Log("Timer is: " + shakeTimer);
            shakeTimer -= Time.unscaledDeltaTime;
            noise.m_AmplitudeGain =  Mathf.Lerp(startIntensity, 0f, shakeTimer / shakeTimerTotal);
        }
        else
        {
            noise.m_AmplitudeGain = 0f;
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        noise.m_AmplitudeGain = intensity;

        startIntensity = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
    }

    void HandleOnGameOver()
    {
        CinemachineBasicMultiChannelPerlin noise = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f;
        shakeTimer = 0f;
    }
}
