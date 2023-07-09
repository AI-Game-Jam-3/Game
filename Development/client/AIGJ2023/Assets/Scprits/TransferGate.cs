using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferGate : MonoBehaviour
{
    public TransferGate TargetGate;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(TargetGate != null, "TargetGate is null");
    }

    public void Transfer(Transform transform)
    {
        transform.position = new Vector3(TargetGate.transform.position.x, TargetGate.transform.position.y, transform.position.z);
    }


    // Update is called once per frame
    void Update()
    {

    }


}
