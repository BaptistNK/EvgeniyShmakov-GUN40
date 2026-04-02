using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Renderer cellRenderer;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color highlightedColor = Color.yellow;

    public Unit UnitOnCell { get; private set; }
    public Vector2Int GridPosition { get; set; }
    private bool isHighlighted = false;

    public void SetUnit(Unit unit)
    {
        UnitOnCell = unit;
        if (unit != null)
            unit.SetCell(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHighlighted && UnitOnCell == null && cellRenderer != null)
        {
            Highlight();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHighlighted)
        {
            RemoveHighlight();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleController.Instance?.HandleCellClick(this);
    }

    public void Highlight()
    {
        isHighlighted = true;
        cellRenderer.material.color = highlightedColor;
    }

    public void RemoveHighlight()
    {
        isHighlighted = false;
        cellRenderer.material.color = defaultColor;
    }
}
