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
    private const int frameCount = 60;
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
    public bool isPlaying;

    IEnumerator QuitAnimationPhase1()
    {
        isPlaying = true;
        var frame = QuitPhase1Time * frameCount;
        for (int i = 0; i < frame; i++)
        {
            foreach(var renderer in spriteRenderers)
            {
                var color = LightingColor;
                color.a = (float)i/ frame;
                Material material = renderer.material;
                material.SetColor("_BaseColor", color);
                renderer.color = color;
                virtualCamera.m_Lens.OrthographicSize += cameraZoomDeltaAbs / frame;
            }
            yield return new WaitForSecondsRealtime(QuitPhase1Time / frame);
        }
        StartCoroutine(QuitAnimationPhase2());
    }

    IEnumerator QuitAnimationPhase2()
    {
        var frame = QuitPhase2Time * frameCount;
        for (int i = 0; i < frame; i++)
        {
            virtualCamera.m_Lens.OrthographicSize += maxCameraZoom / frame;
            backgroundColor.a = (float)i/frame;
            backgroundRender.color = backgroundColor;
            yield return new WaitForSecondsRealtime(QuitPhase2Time / frame);
        }
        isPlaying = false;
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

    IEnumerator EnterAnimation()
    {
        isPlaying = true;
        var frame = frameCount * EnterTime;
        for(int i = 1; i <= frame; i++)
        {
            var color = Color.white;
            color.a = (float)i/frame;
            characterRenderer.color = color;
            characterRenderer.size = characterSize * i / frame;
            yield return new WaitForSecondsRealtime(EnterTime / frame);
        }
        isPlaying = false;
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
