using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageDisplay : MonoBehaviour
{
    public string message;
    public bool triggered = false;
    public event Action onEnter;
    public event Action onExit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<SmallTrigger>(out var comp) && !triggered)
        {
            UIManager.Instance.messageBox.ShowMessage(message);
            onEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.TryGetComponent<SmallTrigger>(out var comp))
        {
            UIManager.Instance.messageBox.ShowMessage("");
            triggered = true;
            onExit?.Invoke();
        }
    }
}
