using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    //响应游戏开始事按钮件
    public void OnButtonGameStart()
    {
        SceneManager.LoadScene("Demo01");  //读取关卡level1
    }
}