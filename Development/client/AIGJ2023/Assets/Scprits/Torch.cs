using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool IsLighting = false;
    public int LightRadius = 1;

    public void LightAround()
    {
        IsLighting = true;
        for(int y = -LightRadius; y <= LightRadius; y++)
        {
            for(int x = -LightRadius; x <= LightRadius; x++)
            {
                var currentCoord = MapManager.Instance.Pos2Coord(transform.position);
                var newCoord = currentCoord + new Vector2(x, y);
                if(MapManager.Instance.IsOutSide((int)newCoord.x, (int)newCoord.y))
                {
                    continue;
                }
                MapUnit unit = MapManager.Instance.currentMap.GetUnit(newCoord);
                if(unit != null)
                {
                    // TODO: 光照效果
                    MapManager.Instance.TorchUnitsGroup.Add(unit);
                    //unit.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
