using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public List<GameObject> Positions;
    public List<GameObject> Buttons;
    public GameObject Cursor;
    public int index = 0;
    private void Start() {

    }
    private void Update() {
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
        Debug.Log("EnterCollection");
    }

    public void OpenStaff()
    {
        Debug.Log("OpenStaff");
    }

    public void ExitGame()
    {
        Debug.Log("Quit");
        // Application.Quit();
    }
}
