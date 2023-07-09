using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AchievementSystem : MonoBehaviour
{
    public List<Achievement> achievements;
    public GameObject uiPrefab;
    private GameObject ui;
    public GameObject UI => ui;

    public void SetAchievementStatus(string title, bool isUnlock = true)
    {
        foreach (var achievement in achievements)
        {
            if (achievement.Title == title) { achievement.Unlock = isUnlock; }
        }
    }

    public void Show()
    {
        UIManager.Instance?.HideOthers(this);

        ui = GameObject.Instantiate(uiPrefab);
        ui.transform.SetParent(this.transform);
        ui.name = "AchievementSystemUI";

        var button = GameObject.Find("Button");
        var panelRect = GameObject.Find("Content").GetComponent<RectTransform>();
        var buttonRect = GameObject.Find("Button").GetComponent<RectTransform>();
        var text = GameObject.Find("InputField").GetComponentInChildren<Text>();
        var closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
        var icon = GameObject.Find("Icon").GetComponent<Image>();
        var title = GameObject.Find("Title").GetComponent<Text>();
        title.text = "";
        text.text = "";
        closeButton.onClick.AddListener(Close);

        var buttonGap = -buttonRect.anchoredPosition.y;
        var buttonHeight = buttonRect.sizeDelta.y;
        var panelHeight = achievements.Count * (buttonGap + buttonHeight) + buttonGap;
        var panelWidth = panelRect.sizeDelta.x;
        panelRect.sizeDelta = new Vector2(panelWidth, panelHeight);

        bool isShowAchievementContent = false;

        for (int i = 0; i < achievements.Count; i++)
        {
            var index = i;
            var buttonObject = GameObject.Instantiate(button);
            buttonObject.name = "Achievement" + i.ToString();
            buttonObject.transform.SetParent(button.transform.parent);
            buttonObject.transform.localScale = Vector3.one;
            buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * (buttonGap + buttonHeight) - buttonGap);

            if (achievements[i].Unlock)
            {
                buttonObject.GetComponentInChildren<Text>().text = achievements[i].Title;
                buttonObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log(achievements[index].Content);
                    text.text = achievements[index].Content;
                    text.fontSize = 30;
                    text.lineSpacing = 1.1f;
                    icon.sprite = achievements[index].Icon;
                    icon.color = Color.white;
                    title.text = achievements[index].Title;
                });
                if (!isShowAchievementContent)
                {
                    text.text = achievements[index].Content;
                    text.fontSize = 30;
                    text.lineSpacing = 1.1f;
                    icon.sprite = achievements[index].Icon;
                    icon.color = Color.white;
                    title.text = achievements[index].Title;
                    isShowAchievementContent = true;
                }
            }
            else
            {
                buttonObject.GetComponentInChildren<Text>().text = "???";
                buttonObject.GetComponent<Button>().interactable = false;
            }

        }
        GameObject.Destroy(button);
    }

    public void Close()
    {
        UIManager.Instance?.ShowOthers();
        GameObject.Destroy(ui);
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name != "StartScene")
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (ui == null && UIManager.Instance.currentPanel == null)
                    Show();
                else if (ui != null)
                    Close();
            }
            else if(Input.GetKeyDown(KeyCode.Escape) && ui != null)
            {
                Close();
            }
        }
    }
}
