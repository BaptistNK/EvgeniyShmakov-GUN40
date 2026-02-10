using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ReadOnlyAttribute attr = (ReadOnlyAttribute)attribute;

        var labelWithTooltip = new GUIContent(label.text, attr.Tooltip);

        GUI.color = EditorGUIUtility.isProSkin
            ? new Color(1f, 1f, 1f, 1f)
            : new Color(0f, 0f, 0f, 1f);

        EditorGUI.LabelField(position, labelWithTooltip);

        position.xMin += EditorGUI.GetPropertyHeight(property, label, true) + 2f;

        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                EditorGUI.LabelField(position, property.intValue.ToString());
                break;
            case SerializedPropertyType.Float:
                EditorGUI.LabelField(position, property.floatValue.ToString());
                break;
            case SerializedPropertyType.Boolean:
                EditorGUI.LabelField(position, property.boolValue.ToString());
                break;
            case SerializedPropertyType.String:
                EditorGUI.LabelField(position, property.stringValue);
                break;
            case SerializedPropertyType.Enum:
                EditorGUI.LabelField(position, property.enumNames[property.enumValueIndex]);
                break;
            default:
                EditorGUI.LabelField(position,
                    property.displayName + ": " +
                    (property.objectReferenceValue?.ToString() ?? "null"));
                break;
        }

        GUI.color = Color.white;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
