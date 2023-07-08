using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Button openButton;
    public string mainScene;
    public GameObject uiPrefab;
    private GameObject ui;

    private void Awake()
    {
    }

    private void Start()
    {
        openButton.onClick.AddListener(() =>
        {
            if (ui == null && !AchievementSystem.Instance.HasPanel)
                Show();
        });
    }

    public void Show()
    {
        ui = GameObject.Instantiate(uiPrefab);
        ui.transform.SetParent(this.transform);
        ui.name = "SettingUI";

        var restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        var quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        // var controlButton = GameObject.Find("ControlButton").GetComponent<Button>();
        var returnButton = GameObject.Find("ReturnButton").GetComponent<Button>();
        var controlPanel = GameObject.Find("ControlPanel");
        // controlPanel.SetActive(false);

        restartButton.onClick.AddListener(() => { var sceneName = SceneManager.GetActiveScene().name; SceneManager.LoadScene(sceneName); });
        quitButton.onClick.AddListener(() => { SceneManager.LoadScene(mainScene); });
        // controlButton.onClick.AddListener(() => { controlPanel.SetActive(true); });
        // returnButton.onClick.AddListener(() => { controlPanel.SetActive(false); });
        returnButton.onClick.AddListener(() => { Close(); });

        MessageBox.Instance.gameObject.SetActive(false);
        AchievementSystem.Instance.HasPanel = true;
    }

    public void Close()
    {
        AchievementSystem.Instance.HasPanel = false;
        MessageBox.Instance.gameObject.SetActive(true);
        GameObject.Destroy(ui);
    }

    // private void Update()
    // {
    //     if(Input.GetKeyUp(KeyCode.Escape))
    //     {
    //         if (ui == null) Show();
    //         else Close();
    //     }
    // }
}
