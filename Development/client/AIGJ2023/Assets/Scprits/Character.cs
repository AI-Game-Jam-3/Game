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

    List<MapUnit> LightedUnitsCache;
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
            PostUpdateMapExplore();
        }
    }

    public void Move()
    {
        var currentMap = MapManager.Instance.currentMap;
        var currentCoord = MapManager.Instance.Pos2Coord(transform.position);
        var targetCoord = MapManager.Instance.CalculateUnitCoord(currentCoord, MoveDirection);
        if(targetCoord == null)
        {
            return;
        }
        var unit = currentMap.GetUnit(targetCoord.Value);
        if(unit != null && unit.UnitType == "Wall")
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

    void PostUpdateMapExplore()
    {
        //Debug.Log("Current Character Position: " + character.transform.position);
        Vector2 CharacterXY = MapManager.Instance.Pos2Coord(this.transform.position);
        int CurX = (int)CharacterXY.x;
        int CurY = (int)CharacterXY.y;

        //List<MapUnit> ExpolreUnits = MapManager.Instance.ExpolreNeighborKernel(7, CurX, CurY);
        //List<MapUnit> LightedUnits = MapManager.Instance.GetLightedArea(3, CurX, CurY);
        ////foreach (MapUnit unit in ExpolreUnits)
        ////{
        ////    Debug.Log("ExpolreUnit: " + unit.transform.position);
        ////}
        ////Debug.Log("=======================");

        List<MapUnit> ExpolreUnits = GetComponent<WaveShooter>().ShootRays(7);

        if (LightedUnitsCache != null && LightedUnitsCache.Count > 0)
        {
            foreach (MapUnit unit in LightedUnitsCache)
            {
                if (unit != null)
                {
                    if (unit.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                    {
                        if(unit.bIsWall)
                        {
                            spriteRenderer.color = Color.blue;
                        }
                        else
                        {
                            spriteRenderer.color = Color.white;
                        }
                        
                    }
                }
            }
        }
        

        foreach (MapUnit unit in ExpolreUnits)
        {
            Debug.Log("LightedUnits: " + unit.transform.position);
            if(unit!=null)
            {
                if(unit.TryGetComponent<SpriteRenderer>(out  SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.color = Color.yellow;   
                }
            }
        }
        LightedUnitsCache = ExpolreUnits;


    }
}
