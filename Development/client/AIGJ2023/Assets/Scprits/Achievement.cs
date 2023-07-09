using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    public string Title = "N/A";
    public Sprite Icon;
    [TextArea]
    public string Content = "N/A";
    public bool Unlock = false;
}
