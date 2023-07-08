using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 管理关卡流程进度
public class GameManager : MonoBehaviour
{
    public List<string> levelNames;
    public int currentLevelIndex = 0;
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
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

        DontDestroyOnLoad(gameObject);
    }

    public void EnterLevel(int index)
    {
        currentLevelIndex = index;
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelNames[index]);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
