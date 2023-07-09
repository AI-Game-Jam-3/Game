using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyCollection : MonoBehaviour
{
    // 飞行速度
    public float Speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("FlyCollection Start");
        // 计算屏幕右下角的世界坐标
        Vector3 screenPos = new Vector3(Screen.width, Screen.height, 0);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos += new Vector3(0.0f, -10.0f, 0);

        // 使用DOTWEEN从当前位置飞向屏幕右下角然后消失
        transform.DOMove(worldPos, Speed).SetDelay(0.5f).OnComplete(() => {
            Destroy(gameObject);
        }).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
