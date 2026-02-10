using UnityEngine;
using UnityEditor;

public class ReadOnlyAttribute : PropertyAttribute 
{
    public string Tooltip { get; private set; }

    public ReadOnlyAttribute(string tooltip = "")
    {
        Tooltip = tooltip;
    }
}
