using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnim : MonoBehaviour
{

    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;

    public float animationSpeed;
    public GameObject panel;

    public bool isGoingToClose;

    // Start is called before the first frame update
    void Start()
    {
        isGoingToClose = false;
        StartCoroutine(ShowPanel(panel));
    }

    // Update is called once per frame
    void Update()
    {
        // ��������ת����
        if(isGoingToClose && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(HidePanel(panel));
        }
    }


    IEnumerator ShowPanel(GameObject GO)
    {
        float timer = 0;
        while (timer <= 1)
        {
            GO.transform.localScale = Vector3.one * showCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    IEnumerator HidePanel(GameObject GO)
    {
        isGoingToClose = false;
        float timer = 0;
        while (timer < 1)
        {
            GO.transform.localScale = Vector3.one * hideCurve.Evaluate(timer);
            GO.transform.localScale.Clamp(Vector3.zero, Vector3.one);
            timer += Time.deltaTime * animationSpeed;
            yield return new WaitForFixedUpdate();
        }
        GO.transform.localScale = Vector3.zero;
    
        GameManager.Instance.EnterLevel(5);
   
        

    }




}
