using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAnimation : MonoBehaviour
{
    public float EnterTime = 2.0f;
    public float QuitPhase1Time = 2.0f;
    public float QuitPhase2Time = 2.0f;
    public Color LightingColor = Color.white;
    public float CameraZoomDelta = 0.2f;
    private const int frameCount = 255;
    private SpriteRenderer[] spriteRenderers;
    private CinemachineVirtualCamera virtualCamera;
    private float cameraZoomDeltaAbs;
    private const float maxCameraZoom = 100;
    private SpriteRenderer backgroundRender;
    private Color backgroundColor;
    private SpriteRenderer characterRenderer;
    private Vector2 characterSize;


    public delegate void Handler();
    public event Handler QuitAnimationFinish;
    //public event Handler EnterAnimationFinish;

    IEnumerator QuitAnimationPhase1()
    {
        for(int i = 0; i < frameCount; i++)
        {
            foreach(var renderer in spriteRenderers)
            {
                var color = LightingColor;
                color.a = (float)i/frameCount;
                renderer.color = color;
                virtualCamera.m_Lens.OrthographicSize += cameraZoomDeltaAbs / frameCount;
            }
            yield return new WaitForSecondsRealtime(QuitPhase1Time / frameCount);
        }
        StartCoroutine(QuitAnimationPhase2());
    }

    IEnumerator QuitAnimationPhase2()
    {
        for (int i = 0; i < frameCount; i++)
        {
            virtualCamera.m_Lens.OrthographicSize += maxCameraZoom / frameCount;
            backgroundColor.a = (float)i/frameCount;
            backgroundRender.color = backgroundColor;
            yield return new WaitForSecondsRealtime(QuitPhase2Time / frameCount);
        }
        QuitAnimationFinish();
    }

    public void TriggerQuitAnimation()
    {
        GameObject.Find("UICanvas").SetActive(false);
        spriteRenderers = GameObject.Find("Wall").GetComponentsInChildren<SpriteRenderer>();
        virtualCamera = GameObject.Find("VCam").GetComponent<CinemachineVirtualCamera>();
        backgroundRender = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        backgroundRender.sortingOrder = 100;
        backgroundColor = backgroundRender.color;

        var camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        camera.backgroundColor = backgroundColor;
        camera.clearFlags = CameraClearFlags.SolidColor;

        backgroundColor.a = 0;
        backgroundRender.color = backgroundColor;
        cameraZoomDeltaAbs = CameraZoomDelta * virtualCamera.m_Lens.OrthographicSize;
        StartCoroutine(QuitAnimationPhase1());
    }

    private IEnumerator EnterAnimation()
    {
        for(int i = 1; i <= frameCount; i++)
        {
            var color = Color.white;
            color.a = (float)i/frameCount;
            characterRenderer.color = color;
            characterRenderer.size = characterSize * i / frameCount;
            yield return new WaitForSecondsRealtime(EnterTime / frameCount);
        }
        //EnterAnimationFinish();
    }

    public void TriggerEnterAnimation()
    {
        characterRenderer = GameObject.Find("Character").GetComponent<SpriteRenderer>();
        characterSize = characterRenderer.size;
        StartCoroutine(EnterAnimation());
    }

    public void Start()
    {
        TriggerEnterAnimation();
    }
}
