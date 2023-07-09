using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TextGraduallyOut : MonoBehaviour
{
    // Start is called before the first frame update
    public Text displayMessage;

    public GameObject PlayerAnimInstance;

    [Multiline]public string inputMessage;

    public float waitTime = 0.2f;

    void Start()
    {
        StartCoroutine(TypeText(displayMessage, inputMessage, waitTime));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator TypeText(Text tMP_Text, string str, float interval)
    {
        if(tMP_Text == null)
        {
            Debug.LogWarning("Text component is not set");
            yield break;
        }
        int i = 0;
        while( i <= str.Length)
        {
            tMP_Text.text = str.Substring(0, i++);
            yield return new WaitForSeconds(interval);
        }
        if(PlayerAnimInstance == null)
        {
            Debug.LogWarning("Player Out Controller is not set");
            yield break;
        }
        PlayerAnimInstance.GetComponent< PanelAnim >().isGoingToClose = true;
        yield break;
    }

}
