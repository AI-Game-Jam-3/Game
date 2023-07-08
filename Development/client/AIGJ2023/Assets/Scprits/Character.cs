using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Character : MonoBehaviour
{
    [LabelText("移动方向")]
    public Vector2 MoveDirection;
    [LabelText("原始位置")]
    public Vector3 OriginPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MoveDirection != Vector2.zero)
        {
            Move();
        }
    }

    public void Move()
    {
        var currentMap = MapManager.Instance.currentMap;
        var currentCoord = MapManager.Instance.Pos2Coord(transform.position);
        var targetCoord = MapManager.Instance.CalculateUnitCoord(currentCoord, MoveDirection);
        // if(targetCoord != null)
        // {
        //     Debug.Log(targetCoord + ": " + currentMap.GetUnit(targetCoord.Value).UnitType);
        // }
        if(targetCoord == null || currentMap.GetUnit(targetCoord.Value).UnitType == "Wall")
        {
            return;
        }
        transform.Translate(MoveDirection * MapManager.Instance.CELL_SIZE);
    }

    [Button("返回原始位置")]
    void BackToOriginPos()
    {
        transform.position = OriginPos;
    }
}
