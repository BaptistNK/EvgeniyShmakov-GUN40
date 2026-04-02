using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Renderer unitRenderer;
    [SerializeField] private Color selectedColor = Color.green;

    public Cell CurrentCell { get; private set; }
    public bool IsWhite { get; private set; }
    public bool IsKing { get; set; }

    private Color defaultColor;
    private bool isSelected = false;

    public void Initialize(bool isWhite)
    {
        IsWhite = isWhite;
        defaultColor = unitRenderer?.material.color ?? Color.white;
    }

    public void SetCell(Cell cell)
    {
        CurrentCell = cell;
        // Обновляем позицию объекта, чтобы он находился над клеткой
        if (cell != null)
        {
            transform.position = cell.transform.position + new Vector3(0, 0.5f, 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected && unitRenderer != null)
        {
            unitRenderer.material.color = selectedColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected && unitRenderer != null)
        {
            unitRenderer.material.color = defaultColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleController.Instance?.HandleUnitClick(this);
    }

    public void Select()
    {
        isSelected = true;
        if (unitRenderer != null)
            unitRenderer.material.color = selectedColor;
    }

    public void Deselect()
    {
        isSelected = false;
        if (unitRenderer != null)
            unitRenderer.material.color = defaultColor;
    }
}
