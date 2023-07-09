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
        renderer.color = Color.white;
        renderer.sortingOrder = 10;
        StartCoroutine(AnimatorController());
    }

    IEnumerator AnimatorController()
    {
        do
        {
            foreach (var frame in frames)
            {
                if (renderer != null)
                {
                    renderer.sprite = frame;
                    renderer.size = size * Vector2.one;
                }
                yield return new WaitForSecondsRealtime(1.0f / framerate);
            }
        } while (alwaysShow);
        GameObject.Destroy(echoObject);
    }

    void Start()
    {
        if(playOnAwake)
            TriggerEchoAnimation();
    }
}
