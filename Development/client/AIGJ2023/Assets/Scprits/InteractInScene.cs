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


    private void OnTriggerEnter2D(Collider2D other)
    {
        // // 判断该物体的方位
        // var dir = new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y);
        // var unit = other.GetComponentInChildren<MapUnit>();
        // if (unit == null)
        // {
        //     return;
        // }

        // var torch = unit.GetComponentInChildren<Torch>();
        // if (torch != null && !torch.IsLighting)
        // {
        //     CanFireOrBreak = true;
        //     FireOrBreak += () => {
        //         torch.LightAround();
        //     };
        //     // if (CanFireOrBreak)
        //     // {
        //     //     torch.LightAround();
        //     //     Destroy(collider2Button[other].gameObject);
        //     //     collider2Button.Remove(other);
        //     // }
        //     // var button = ActiveButton("点亮火把");
        //     // collider2Button.Add(other, button);
        //     // button.onClick.AddListener(() =>
        //     // {
        //     //     torch.LightAround();
        //     //     Destroy(collider2Button[other].gameObject);
        //     //     collider2Button.Remove(other);
        //     // });
        // }

        // var breakableWall = unit.GetComponentInChildren<BreakableWall>();
        // if (breakableWall != null)
        // {
        //     CanFireOrBreak = true;
        //     FireOrBreak += () => {
        //         breakableWall.GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
        //         breakableWall.breaked = true;
        //     };
        //     // if (CanFireOrBreak)
        //     // {
        //     //     breakableWall.gameObject.SetActive(false);
        //     //     Destroy(collider2Button[other].gameObject);
        //     //     collider2Button.Remove(other);
        //     // }
        //     // Debug.Log("BreakableWall");
        //     // var button = ActiveButton("破坏墙壁");
        //     // collider2Button.Add(other, button);
        //     // button.onClick.AddListener(() =>
        //     // {
        //     //     breakableWall.gameObject.SetActive(false);
        //     //     Destroy(collider2Button[other].gameObject);
        //     //     collider2Button.Remove(other);
        //     // });
        // }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if (collider2Button.ContainsKey(other))
        // {
        //     CanFireOrBreak = false;
        //     FireOrBreak = null;
        //     Destroy(collider2Button[other].gameObject);
        //     collider2Button.Remove(other);
        // }
    }
}
