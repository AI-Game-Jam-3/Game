using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public int nextLevelIndex = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<SmallTrigger>(out var comp))
        {
            GameManager.Instance?.EnterLevel(nextLevelIndex);
        }
    }
}
