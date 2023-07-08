using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
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

}

#if UNITY_EDITOR

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var mapManager = target as MapManager;

        if (GUILayout.Button("读取地图"))
        {
            mapManager.ReadMap();
        }
    }

    [System.Obsolete("别用")]
    void ReadMap()
    {
        var mapManager = target as MapManager;
        var currentMap = serializedObject.FindProperty("currentMap").objectReferenceValue as RectMap;
        serializedObject.Update();

        currentMap.Width = 0;
        currentMap.Height = 0;
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

        foreach (var name in mapUnitsDict.Keys)
        {
            var mapUnits = mapUnitsDict[name];

            // 遍历所有子物体
            foreach (GameObject mapUnit in mapUnits)
            {
                var coord = mapManager.Pos2Coord(mapUnit.transform.position);
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
                var coord = mapManager.Pos2Coord(mapUnit.transform.position);
                var x = (int)coord.x;
                var y = (int)coord.y;
                Debug.Assert(currentMap.Width > x && currentMap.Height > y, "地图越界");
                currentMap.MapUnits[y, x] = mapUnit.GetComponent<MapUnit>();
                currentMap.MapUnits[y, x].UnitType = name;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}


#endif
