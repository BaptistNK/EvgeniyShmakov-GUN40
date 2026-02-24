using UnityEngine;

[CreateAssetMenu(fileName = "New CellPaletteSettings", menuName ="Settings/CellPaletteSettings", order = 52)]
public class CellPalletteSettings : ScriptableObject
{
    [field: SerializeField, Space(20f)]
    [field: Tooltip("Клетка под выбранным юнитом")]
    public Material SelectCell {  get; private set; }
    [field: SerializeField]
    [field: Tooltip("Клетка доступна для передвижения")]
    public Material MoveCell { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Клетка доступна для атаки")]
    public Material AttackCell { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Клетка доступна и для передвижения и для атаки")]
    public Material MoveAndAttackMoveCell { get; private set; }
}
