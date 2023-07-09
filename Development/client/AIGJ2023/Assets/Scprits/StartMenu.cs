using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public bool IsPanelOpen;
    public List<GameObject> Positions;
    public List<GameObject> Buttons;
    public GameObject Cursor;
    public int index = 0;
    public AchievementSystem achievementSystem;
    public GameObject Credit;
    public Button CreditBackButton;
    private void Start() {
        CreditBackButton.onClick.AddListener(() => {
            Credit.SetActive(false);
        });
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Credit.activeSelf)
            {
                Credit.SetActive(false);
                IsPanelOpen = false;
            }
            else if(achievementSystem.UI != null)
            {
                achievementSystem.Close();
                IsPanelOpen = false;
            }
        }


        if(achievementSystem.UI == null && !Credit.activeSelf)
        {
            IsPanelOpen = false;
        }

        if(IsPanelOpen)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            index--;
            if(index < 0)
            {
                index = 0;
            }
            else
            {
                Cursor.transform.position = Positions[index].transform.position;
            }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            index++;
            if(index > 3)
            {
                index = 3;
            }
            else
            {
                Cursor.transform.position = Positions[index].transform.position;
            }
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            switch(index)
            {
                case 0:
                    EnterGame();
                    break;
                case 1:
                    EnterCollection();
                    break;
                case 2:
                    OpenStaff();
                    break;
                case 3:
                    ExitGame();
                    break;
            }
        }
    }

    public void EnterGame()
    {
        GameManager.Instance.EnterLevel(0);
    }

    public void EnterCollection()
    {
        if(achievementSystem.UI == null)
        {
            IsPanelOpen = true;
            achievementSystem.Show();
        }
        Debug.Log("EnterCollection");
    }

    public void OpenStaff()
    {
        if(!Credit.activeSelf)
        {
            Credit.SetActive(true);
            IsPanelOpen = true;
        }
        Debug.Log("OpenStaff");
    }

    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
