using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MapManager>();
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
            Destroy(gameObject);
        }
    }

    public int CELL_SIZE = 1;
    public RectMap currentMap;
    public Vector3 Coord2Pos(Vector2 coord)
    {
        // (0, 0) => (SIZE/2, -SIZE/2)
        // y轴相反
        return new Vector3(coord.x * CELL_SIZE + CELL_SIZE / 2, -coord.y * CELL_SIZE - CELL_SIZE / 2);
    }

    public Vector2 Pos2Coord(Vector3 pos)
    {
        // (SIZE/2, -SIZE/2) => (0, 0)
        // y轴相反
        return new Vector2((pos.x - CELL_SIZE / 2) / CELL_SIZE, -(pos.y + CELL_SIZE / 2) / CELL_SIZE);
    }
}
