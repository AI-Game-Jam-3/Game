using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit : MonoBehaviour
{

    public bool bIsWall = false;

    public bool CanMove
    {
        get
        {
            if(UnitType == "Wall")
            {
                return false;
            }
            if(UnitType == "BreakableWall" && !GetComponent<BreakableWall>().breaked)
            {
                return false;
            }
            return true;
        }
    }

    public string UnitType;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TryExplore(MapUnit TargetUnit)
    {
        Debug.Log("This GO Transform Location: " + this.transform.position);
        Debug.Log("Character Go Tranform Location: " + TargetUnit.transform.position);
        return true;
    }
}
