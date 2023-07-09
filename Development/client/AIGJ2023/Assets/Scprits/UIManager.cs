using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    private void Awake() {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Setting setting;
    public MessageBox messageBox;
    public AchievementSystem achievementSystem;
    public MonoBehaviour currentPanel;
    public List<MonoBehaviour> panels = new List<MonoBehaviour>();

    // Start is called before the first frame update
    void Start()
    {
        panels.Add(setting);
        panels.Add(messageBox);
        panels.Add(achievementSystem);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideOthers(MonoBehaviour cur)
    {
        currentPanel = cur;
        foreach(var panel in panels)
        {
            if(panel != currentPanel)
            {
                (panel as MonoBehaviour).gameObject.SetActive(false);
            }
        }
    }

    public void ShowOthers()
    {
        foreach(var panel in panels)
        {
            if(panel != currentPanel)
            {
                (panel as MonoBehaviour).gameObject.SetActive(true);
            }
        }
        currentPanel = null;
    }
}
