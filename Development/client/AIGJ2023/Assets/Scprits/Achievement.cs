using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    public string Title = "N/A";
    public string Content = "N/A";
    public bool Unlock = false;
}
