using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 判断该物体的方位
        var dir = new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y);
        var unit = other.GetComponentInChildren<MapUnit>();
        if(unit == null)
        {
            return;
        }

        var torch = unit.GetComponentInChildren<Torch>();
        if (torch != null && !torch.IsLighting)
        {
            var button = ActiveButton("点亮火把");
            collider2Button.Add(other, button);
            button.onClick.AddListener(() =>
            {
                torch.LightAround();
                Destroy(collider2Button[other].gameObject);
                collider2Button.Remove(other);
            });
        }

        Debug.Log(unit.UnitType);
        var breakableWall = unit.GetComponentInChildren<BreakableWall>();
        if(breakableWall != null)
        {
            Debug.Log("BreakableWall");
            var button = ActiveButton("破坏墙壁");
            collider2Button.Add(other, button);
            button.onClick.AddListener(() =>
            {
                breakableWall.gameObject.SetActive(false);
                Destroy(collider2Button[other].gameObject);
                collider2Button.Remove(other);
            });
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (collider2Button.ContainsKey(other))
        {
            Destroy(collider2Button[other].gameObject);
            collider2Button.Remove(other);
        }
    }
}
