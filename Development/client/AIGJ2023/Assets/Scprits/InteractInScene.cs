using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InteractInScene : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public GameObject Panel;
    public GameObject Pivot;
    public Dictionary<Collider2D, Button> collider2Button;
    // Start is called before the first frame update
    void Start()
    {
        collider2Button = new Dictionary<Collider2D, Button>();
    }


    // Update is called once per frame
    void Update()
    {
        // 将当前位置转换到屏幕位置
        Vector3 screenPos = Camera.main.WorldToScreenPoint(Pivot.transform.position);
        // 将button的位置设置为当前位置
        Panel.transform.position = screenPos;
    }

    Button ActiveButton(string text)
    {
        var button = Instantiate(ButtonPrefab, Panel.transform).GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = text;
        return button;
    }

    IEnumerable<(int, int)> GetXY3x3()
    {
        for(int y = -1; y <= 1; y++)
        {
            for(int x = -1; x <= 1; x++)
            {
                yield return (x, y);
            }
        }
    }

    public void Interactive()
    {
        for(int y = -1; y <= 1; y++)
        {
            for(int x = -1; x <= 1; x++)
            {
                var dir = new Vector2(x, y);
                var currentCoord = MapManager.Instance.Pos2Coord(transform.position);
                var newCoord = currentCoord + dir;
                if (MapManager.Instance.IsOutSide((int)newCoord.x, (int)newCoord.y))
                {
                    continue;
                }
                MapUnit targetUnit = MapManager.Instance.currentMap.GetUnit(newCoord);
                if(targetUnit == null)
                {
                    continue;
                }
                var torch = targetUnit.GetComponentInChildren<Torch>();
                if (torch != null && !torch.IsLighting)
                {
                    torch.LightAround();
                }

                var breakableWall = targetUnit.GetComponentInChildren<BreakableWall>();
                if (breakableWall != null && !breakableWall.breaked)
                {
                    var renderer = breakableWall.GetComponentInChildren<SpriteRenderer>();
                    breakableWall.breaked = true;
                    breakableWall.GetComponentInChildren<MapUnit>().bIsWall = false;
                    renderer.sprite = breakableWall.breakedSprite;
                }
            }
        }
    }

    public void InteractWithFire()
    {
        foreach(var v in GetXY3x3())
        {
            var dir = new Vector2(v.Item1, v.Item2);
            var currentCoord = MapManager.Instance.Pos2Coord(transform.position);
            var newCoord = currentCoord + dir;
            if (MapManager.Instance.IsOutSide((int)newCoord.x, (int)newCoord.y))
            {
                continue;
            }
            MapUnit targetUnit = MapManager.Instance.currentMap.GetUnit(newCoord);
            if(targetUnit == null)
            {
                continue;
            }
            var torch = targetUnit.GetComponentInChildren<Torch>();
            if (torch != null && !torch.IsLighting)
            {
                torch.LightAround();
            }
        }
    }

    public void InteractWithBreakableWall()
    {
        foreach(var v in GetXY3x3())
        {
            var dir = new Vector2(v.Item1, v.Item2);
            var currentCoord = MapManager.Instance.Pos2Coord(transform.position);
            var newCoord = currentCoord + dir;
            if (MapManager.Instance.IsOutSide((int)newCoord.x, (int)newCoord.y))
            {
                continue;
            }
            MapUnit targetUnit = MapManager.Instance.currentMap.GetUnit(newCoord);
            if(targetUnit == null)
            {
                continue;
            }
            var breakableWall = targetUnit.GetComponentInChildren<BreakableWall>();
            if (breakableWall != null && !breakableWall.breaked)
            {
                AudioPlayer.Instance.PlayClip("breakWall");
                var renderer = breakableWall.GetComponentInChildren<SpriteRenderer>();
                breakableWall.breaked = true;
                breakableWall.GetComponentInChildren<MapUnit>().bIsWall = false;
                renderer.sprite = breakableWall.breakedSprite;
            }
        }
    }
}
