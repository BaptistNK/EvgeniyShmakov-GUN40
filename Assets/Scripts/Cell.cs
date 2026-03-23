using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Unit Unit { get; set; }
    public event Action<Cell> OnPointerClickEvent;
    [SerializeField] private MeshRenderer _focus;
    [SerializeField] private MeshRenderer _select;
    private Dictionary<NeighbourType, Cell> _neighbours = new Dictionary<NeighbourType, Cell>();
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClickEvent?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_focus != null)
            _focus.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_focus != null)
            _focus.enabled = false;
    }

    public void SetSelect(Material material)
    {
        if (_select != null)
        { 
            _select.enabled = true;
            GetComponent<Renderer>().material = material;
        }
    }

    public void ResetSelect()
    {
        _select.enabled = false;
    }

    public void AddNeighbour(NeighbourType type,  Cell neighbour)
    {
        if(!_neighbours.ContainsKey(type))
        {
            _neighbours[type] = neighbour;
        }
        else
        {
            Debug.LogWarning($"Клетка уже имеет соседа типа {type}!");
        }
    }

    public Cell GetNeighbour(NeighbourType type)
    {
        return _neighbours.TryGetValue(type, out var cell) ? cell : null;
    }
}
