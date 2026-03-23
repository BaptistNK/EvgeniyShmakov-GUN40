using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Cell Cell { get; private set; }

    private Action<Cell> OnMoveEndCallback;

    public void SetSelect(Cell cell)
    {
        if(Cell!=null)
            Cell.ResetSelect();

        Cell=cell;
        if (Cell != null && cell.Unit != this) 
            cell.Unit=this;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Cell != null)
        {
            Debug.Log($"Клик по юниту на клетке: {Cell.gameObject.name}");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Cell != null)
        {
            Cell.SetSelect(Resources.Load<Material>("HighlightMaterial"));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Cell != null)
        {
            Cell.ResetSelect();
        }
    }

    public void Move(Cell cell)
    {
        if (cell == null || cell.Unit != null) return;
        var startCell = Cell;
        SetSelect(cell);
        transform.position = cell.transform.position;
        startCell?.ResetSelect();
        OnMoveEndCallback?.Invoke(cell);
    }
}
