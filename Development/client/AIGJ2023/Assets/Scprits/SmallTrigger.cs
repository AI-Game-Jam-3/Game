using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTrigger : MonoBehaviour
{
    public Character character;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if(other.TryGetComponent<TransferGate>(out var gate))
        // {
        //     Debug.Log("Enter at: " + gate.name);
        //     character.CanTransfer = true;
        //     character.CurrentGate = gate;
        // }
    }

    private void OnTriggerExit2D(Collider2D other) {
        // if(other.TryGetComponent<TransferGate>(out var gate))
        // {
        //     Debug.Log("Exit at: " + gate.name);
        //     character.CanTransfer = false;
        //     character.CurrentGate = null;
        // }
    }
}
