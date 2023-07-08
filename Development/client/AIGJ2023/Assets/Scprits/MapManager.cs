using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Drawing;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    private void Start() {
        ReadMap();

        SetMapColor();

        TorchUnitsGroup = new List<MapUnit>();
    }

    public int CELL_SIZE = 1;
    public RectMap currentMap;
    public List<MapUnit> TorchUnitsGroup;
    public Vector3 Coord2Pos(Vector2 coord)
    {
        // (0, 0) => (SIZE/2, -SIZE/2)
        // y轴相反
        return new Vector3(coord.x * CELL_SIZE + CELL_SIZE / 2.0f, -coord.y * CELL_SIZE - CELL_SIZE / 2.0f);
    }

    Vector2 _Pos2Coord(Vector3 pos)
    {
        // (SIZE/2, -SIZE/2) => (0, 0)
        // y轴相反
        return new Vector2((pos.x - CELL_SIZE / 2.0f) / CELL_SIZE, -(pos.y + CELL_SIZE / 2.0f) / CELL_SIZE);
    }

    public Vector2 Pos2Coord(Vector3 pos)
    {
        var centerPos = new Vector2(0.5f + (int)(pos.x / CELL_SIZE) * CELL_SIZE, -0.5f + (int)(pos.y / CELL_SIZE) * CELL_SIZE);
        return _Pos2Coord(centerPos);
    }

    [Button("t")]
    public void Test()
    {
        Debug.Log(Pos2Coord(new Vector3(1.7f, -1.3f, 0)));
    }

    public Vector3 GetMapUnitPos(Vector2 coord)
    {
        return Coord2Pos(coord);
    }

    Dictionary<string, List<GameObject>> GetMapUnitsDict()
    {
        var gameRoot = currentMap.GetComponent<Transform>().gameObject;

        // 获取所有子物体
        var kinds = new List<GameObject>();
        foreach (Transform child in gameRoot.transform)
        {
            kinds.Add(child.gameObject);
        }
        Dictionary<string, List<GameObject>> mapUnitsDict = new Dictionary<string, List<GameObject>>();
        // 遍历所有类型
        foreach (GameObject kind in kinds)
        {
            var typeName = kind.name;
            var mapUnits = new List<GameObject>();
            foreach (Transform child in kind.transform)
            {
                mapUnits.Add(child.gameObject);
            }

            mapUnitsDict.Add(typeName, mapUnits);
        }
        return mapUnitsDict;
    }

    public void SetMapColor()
    {
        var currentMap = MapManager.Instance.currentMap;
        foreach (MapUnit unit in currentMap.MapUnits)
        {
            if (unit.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                UnityEngine.Color color = UnityEngine.Color.black;
                color.a = 1;
                spriteRenderer.color = color;
            }
        }
    }

    [Button("读取地图")]
    public void ReadMap()
    {
        currentMap.Width = 0;
        currentMap.Height = 0;
        var gameRoot = currentMap.GetComponent<Transform>().gameObject;

        var mapUnitsDict = GetMapUnitsDict();

        foreach (var name in mapUnitsDict.Keys)
        {
            var mapUnits = mapUnitsDict[name];

            // 遍历所有子物体
            foreach (GameObject mapUnit in mapUnits)
            {
                var coord = Pos2Coord(mapUnit.transform.position);
                var x = (int)coord.x;
                var y = (int)coord.y;
                // 取最大的x作为宽度, 取最大的y作为高度
                currentMap.Width = Mathf.Max(currentMap.Width, x + 1);
                currentMap.Height = Mathf.Max(currentMap.Height, y + 1);
            }
        }

        currentMap.MapUnits = new MapUnit[currentMap.Height, currentMap.Width];

        foreach (var name in mapUnitsDict.Keys)
        {
            var mapUnits = mapUnitsDict[name];

            foreach (GameObject mapUnit in mapUnits)
            {
                var coord = Pos2Coord(mapUnit.transform.position);
                var x = (int)coord.x;
                var y = (int)coord.y;
                Debug.Assert(currentMap.Width > x && currentMap.Height > y, "地图越界");
                currentMap.MapUnits[y, x] = mapUnit.GetComponent<MapUnit>();
                currentMap.MapUnits[y, x].UnitType = name;
            }
        }

        // 填充null为new MapUnit()
        // for (int i = 0; i < currentMap.Height; i++)
        // {
        //     for (int j = 0; j < currentMap.Width; j++)
        //     {
        //         if (currentMap.MapUnits[i, j] == null)
        //         {
        //             var GO = new GameObject();
        //             GO.AddComponent<MapUnit>();
        //             currentMap.MapUnits[i, j] = GO.GetComponent<MapUnit>();
        //         }
        //     }
        // }
    }

    public Vector2? CalculateUnitCoord(Vector2 coord, Vector2 moveDir)
    {
        // y轴相反，判断是否越界，越界则返回null
        var x = (int)coord.x + (int)moveDir.x;
        var y = (int)coord.y - (int)moveDir.y;
        if (x < 0 || x >= currentMap.Width || y < 0 || y >= currentMap.Height)
        {
            return null;
        }
        return new Vector2(x, y);
    }

    // 去重位置相同的砖块
    [Button("砖块去重")]
    public void RemoveSamePosBricks()
    {
        var mapUnitsDict = GetMapUnitsDict();
        var hashSet = new HashSet<Vector2>();
        foreach (var name in mapUnitsDict.Keys)
        {
            var mapUnits = mapUnitsDict[name];

            foreach (GameObject mapUnit in mapUnits)
            {
                var coord = Pos2Coord(mapUnit.transform.position);
                if (hashSet.Contains(coord))
                {
                    DestroyImmediate(mapUnit);
                }
                else
                {
                    hashSet.Add(coord);
                }
            }
        }
    }

    public List<MapUnit> GetLightedArea(int KernelSize, int CurX, int CurY)
    {
        List<MapUnit> LighableMapUnits = new List<MapUnit>();
        if (KernelSize % 2 == 0 || currentMap.MapUnits == null)
        {
            // Do not support non odd kernel size
            return LighableMapUnits;
        }
        int Range = KernelSize / 2;

        MapUnit PlayerUnit = currentMap.MapUnits[CurY, CurX];


        // Loop is enough
        for (int i = -Range; i <= Range; ++i)
        {
            for (int j = -Range; j <= Range; ++j)
            {
                int TryX = CurX + i;
                int TryY = CurY + j;
                if (currentMap.MapUnits != null && !IsOutSide(TryX, TryY))
                {
                    LighableMapUnits.Add(currentMap.MapUnits[TryY, TryX]);
                }
            }
        }

        return LighableMapUnits;
    }

    public bool IsOutSide(int X, int Y)
    {
        if (X < 0 || X >= currentMap.Width || Y < 0 || Y >= currentMap.Height)
        {
            return true;
        }
        return false;
    }

    public List<MapUnit> ExpolreNeighborKernel(int KernelSize, int CurX, int CurY)  // kernel size = 7
    {
        List<MapUnit> AccessableMapUnits = new List<MapUnit>();
        if(KernelSize % 2 == 0 || currentMap.MapUnits == null)
        {
            // Do not support non odd kernel size
            return AccessableMapUnits;
        }
        int Range = KernelSize / 2;

        MapUnit PlayerUnit = currentMap.MapUnits[CurY, CurX];

        if (PlayerUnit && currentMap)
        {
            //// TODO: change to DFS
            //for (int i = -Range; i <= Range; ++i)
            //{
            //    for (int j = -Range; j <= Range; ++j)
            //    {
            //        int TryX = CurX + i;
            //        int TryY = CurY + j;w
            //        if (currentMap.MapUnits != null && currentMap.MapUnits[TryX, TryY] != null && PlayerUnit.TryExplore(currentMap.MapUnits[TryX, TryY]))
            //        {
            //            AccessableMapUnits.Add(currentMap.MapUnits[TryX, TryY]);
            //        }
            //    }
            //}

            // TODO: Optimize, not loop all unit, but started with four corner
            Vector3 PlayerPos = PlayerUnit.transform.position;
            for (int i = -Range; i <= Range; ++i)
            {
                for (int j = -Range; j <= Range; ++j)
                {


                    int TryX = CurX + i;
                    int TryY = CurY + j;
                    if(IsOutSide(TryX, TryY))
                    {
                        continue;
                    }
                    MapUnit TargetUnit = currentMap.MapUnits[TryY, TryX];

                    Vector3 TargetPos = TargetUnit.transform.position;
                    Vector3 DistanceVector = PlayerPos - TargetPos;
                    float StepY = DistanceVector.y / DistanceVector.x;
                    float FXSign = DistanceVector.x > 0 ? 1 : -1;
                    float FYSign = DistanceVector.y > 0 ? 1 : -1;
                    float CurrentPosX = TargetPos.x;
                    float CurrentPosY = TargetPos.y;
                    bool bIsVisisble = true;



                    while (CurrentPosX != PlayerPos.x)
                    {
                        Vector2 FIndexXY = Pos2Coord(new Vector3(CurrentPosX, CurrentPosY, 0));
                        MapUnit NextUnit = currentMap.GetUnit(FIndexXY);
                        if (NextUnit != null && NextUnit.bIsWall)
                        {
                            bIsVisisble = false;
                            break;
                        }
                        CurrentPosX += 1f * FXSign;
                        CurrentPosY += StepY;
                    }

                    CurrentPosY = TargetPos.y;
                    CurrentPosX = TargetPos.x;
                    if (CurrentPosX == PlayerPos.x)
                    {
                        while(CurrentPosY != PlayerPos.y)
                        {
                            Vector2 FIndexXY = Pos2Coord(new Vector3(CurrentPosX, CurrentPosY, 0));
                            MapUnit NextUnit = currentMap.GetUnit(FIndexXY);
                            if (NextUnit != null && NextUnit.bIsWall)
                            {
                                bIsVisisble = false;
                                break;
                            }
                            CurrentPosY += 1f * FYSign;
                        }
                    }

                    if (bIsVisisble)
                    {
                        AccessableMapUnits.Add(TargetUnit);
                    }

                }
            }

        }

        return AccessableMapUnits;
    }

}
