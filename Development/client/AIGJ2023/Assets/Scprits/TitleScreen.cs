using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    //��Ӧ��Ϸ��ʼ�°�ť��
    public void OnButtonGameStart()
    {
        SceneManager.LoadScene("Demo01");  //��ȡ�ؿ�level1
    }
}