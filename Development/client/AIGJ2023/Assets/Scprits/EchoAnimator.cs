using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoAnimator : MonoBehaviour
{
    public bool playOnAwake = false;
    public bool alwaysShow = false;
    public float size;
    public float framerate;
    public List<Sprite> frames;
    private GameObject echoObject;
    private SpriteRenderer renderer;

    public void TriggerEchoAnimation()
    {
        echoObject = new GameObject();
        echoObject.name = "EchoAnimation";
        echoObject.transform.SetParent(this.transform);
        echoObject.transform.localPosition = Vector3.zero;
        renderer = echoObject.AddComponent<SpriteRenderer>();
        renderer.drawMode = SpriteDrawMode.Sliced;
        renderer.size = size * Vector2.one;
        renderer.color = Color.white;
        StartCoroutine(AnimatorController());
    }

    IEnumerator AnimatorController()
    {
        while (alwaysShow)
        {
            foreach (var frame in frames)
            {
                if (renderer != null)
                {
                    renderer.sprite = frame;
                }
                yield return new WaitForSecondsRealtime(1.0f / framerate);
            }
        }
        GameObject.Destroy(echoObject);
    }

    void Start()
    {
        if(playOnAwake)
            TriggerEchoAnimation();
    }
}
