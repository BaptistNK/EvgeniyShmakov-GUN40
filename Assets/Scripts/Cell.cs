using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CellState
{
    None,
    Selected,
    CanMove,
    CanAttack,
    CanMoveAndAttack
}

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private MeshRenderer _focusMesh;
    [SerializeField] private MeshRenderer _selectMesh;
    [SerializeField] private CellState currentState = CellState.None;
    public Unit Unit { get; set; } // Ссылка на юнит, стоящий на клетке

    private Dictionary<NeighbourType, Cell> neighbours = new Dictionary<NeighbourType, Cell>();

    public event System.Action<Cell> OnPointerClickEvent;

    private void Awake()
    {
        if (_focusMesh == null)
            _focusMesh = transform.Find("Focus")?.GetComponent<MeshRenderer>();
        if (_selectMesh == null)
            _selectMesh = transform.Find("Select")?.GetComponent<MeshRenderer>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_focusMesh != null)
        {
            _focusMesh.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_focusMesh != null)
        {
            _focusMesh.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClickEvent?.Invoke(this);
    }

    public void SetSelect(Material material)
    {
        if (_selectMesh != null)
        {
            _selectMesh.enabled = true;
            _selectMesh.material = material;
        }
    }

    public void ResetSelect()
    {
        if (_selectMesh != null)
        {
            _selectMesh.enabled = false;
        }

    }

    public void AddNeighbour(Cell neighbour, NeighbourType type)
    {
        if (!neighbours.ContainsKey(type))
        {
            neighbours[type] = neighbour;
        }
    }

    public Cell GetNeighbour(NeighbourType type)
    {
        return neighbours.TryGetValue(type, out Cell neighbour) ? neighbour : null;
    }

    public List<Cell> GetAllNeighbours()
    {
        return new List<Cell>(neighbours.Values);
    }    
}
