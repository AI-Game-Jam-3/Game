using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : MonoBehaviour
{
    public List<Achievement> achievements;
    public GameObject uiPrefab;
    private GameObject ui;

    public void SetAchievementStatus(string title, bool isUnlock = true)
    {
        foreach (var achievement in achievements)
        {
            if (achievement.Title == title) { achievement.Unlock = isUnlock; }
        }
    }

    public void Show()
    {
        ui = GameObject.Instantiate(uiPrefab);
        ui.transform.SetParent(this.transform);
        ui.name = "AchievementSystemUI";

        var button = GameObject.Find("Button");
        var panelRect = GameObject.Find("Content").GetComponent<RectTransform>();
        var buttonRect = GameObject.Find("Button").GetComponent<RectTransform>();
        var text = GameObject.Find("InputField").GetComponentInChildren<Text>();
        text.text = "";

        var buttonGap = -buttonRect.anchoredPosition.y;
        var buttonHeight = buttonRect.sizeDelta.y;
        var panelHeight = achievements.Count * (buttonGap+buttonHeight) + buttonGap;
        var panelWidth = panelRect.sizeDelta.x;
        panelRect.sizeDelta = new Vector2(panelWidth, panelHeight);

        for(int i = 0; i < achievements.Count; i++)
        {
            var index = i;
            var buttonObject = GameObject.Instantiate(button);
            buttonObject.name = "Achievement" + i.ToString();
            buttonObject.transform.SetParent(button.transform.parent);
            buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - i * (buttonGap + buttonHeight) - buttonGap);

            if (achievements[i].Unlock) 
            {
                buttonObject.GetComponentInChildren<Text>().text = achievements[i].Title;
                buttonObject.GetComponent<Button>().onClick.AddListener(() => { text.text = achievements[index].Content; });
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
        GameObject.Destroy(ui);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (ui == null)
                Show();
            else
                Close();
        }
    }

}
