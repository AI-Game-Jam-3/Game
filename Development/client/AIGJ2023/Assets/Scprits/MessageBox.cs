using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{

    private static MessageBox instance;
    public static MessageBox Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MessageBox>();
            }
            return instance;
        }
    }
    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Text messageText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
    }
}
