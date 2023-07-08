using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RectMap : MonoBehaviour
{
    public int Width;
    public int Height;
    [ShowInInspector] public MapUnit[,] MapUnits;

    public MapUnit GetUnit(Vector2 coord)
    {
        return MapUnits[(int)coord.y, (int)coord.x];
    }
}
