using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string cellTag = "Cell";
    [SerializeField] private string unitTag = "Unit";

    public event System.Action<Cell> OnCellClicked;

    private List<Cell> cells = new List<Cell>();
    private Dictionary<Cell, List<Cell>> cellNeighbours = new Dictionary<Cell, List<Cell>>();

    void Start()
    {
        FindAllCells();
        SetupCellNeighbours();
        SubscribeToCellEvents();
        FindAndLinkUnits();
    }

    private void FindAllCells()
    {
        GameObject[] cellObjects = GameObject.FindGameObjectsWithTag(cellTag);
        foreach (GameObject cellObject in cellObjects)
        {
            Cell cell = cellObject.GetComponent<Cell>();
            if (cell != null)
            {
                cells.Add(cell);
            }
        }
        Debug.Log($"Found {cells.Count} cells in scene");
    }

    private void SetupCellNeighbours()
    {
        foreach (Cell cell in cells)
        {
            Vector3 cellPosition = cell.transform.position;
            List<Cell> neighbours = new List<Cell>();

            // Проверяем все 8 возможных направлений (включая диагонали)
            Vector3[] directions = {
                new Vector3(1, 0, 0),   // вправо
                new Vector3(-1, 0, 0),  // влево
                new Vector3(0, 0, 1),   // вверх
                new Vector3(0, 0, -1),  // вниз
                new Vector3(1, 0, 1),  // вправо-вверх (диагональ)
                new Vector3(1, 0, -1), // вправо-вниз (диагональ)
                new Vector3(-1, 0, 1), // влево-вверх (диагональ)
                new Vector3(-1, 0, -1) // влево-вниз (диагональ)
            };

            foreach (Vector3 direction in directions)
            {
                Vector3 neighbourPosition = cellPosition + direction;
                Cell neighbour = FindCellAtPosition(neighbourPosition);
                if (neighbour != null)
                {
                    NeighbourType type = GetNeighbourType(direction);
                    cell.AddNeighbour(neighbour, type);
                    neighbours.Add(neighbour);
                }
            }
            cellNeighbours[cell] = neighbours;
        }
    }

    private Cell FindCellAtPosition(Vector3 position)
    {
        foreach (Cell cell in cells)
        {
            if (Vector3.Distance(cell.transform.position, position) < 0.1f)
            {
                return cell;
            }
        }
        return null;
    }

    private NeighbourType GetNeighbourType(Vector3 direction)
    {
        if (direction.x == 0)
        {
            // Вертикальное направление
            return direction.z > 0 ? NeighbourType.Up : NeighbourType.Down;
        }
        else if (direction.z == 0)
        {
            // Горизонтальное направление
            return direction.x > 0 ? NeighbourType.Right : NeighbourType.Left;
        }
        else
        {
            // Диагональное направление
            if (direction.x > 0 && direction.z > 0) return NeighbourType.UpRight;
            if (direction.x > 0 && direction.z < 0) return NeighbourType.DownRight;
            if (direction.x < 0 && direction.z > 0) return NeighbourType.UpLeft;
            return NeighbourType.DownLeft;
        }
    }

    private void SubscribeToCellEvents()
    {
        foreach (Cell cell in cells)
        {
            cell.OnPointerClickEvent += HandleCellClick;
        }
    }

    private void HandleCellClick(Cell clickedCell)
    {
        OnCellClicked?.Invoke(clickedCell);
    }

    private void FindAndLinkUnits()
    {
        GameObject[] unitObjects = GameObject.FindGameObjectsWithTag(unitTag);
        foreach (GameObject unitObject in unitObjects)
        {
            Unit unit = unitObject.GetComponent<Unit>();
            if (unit != null)
            {
                Cell unitCell = FindCellForUnit(unit);
                if (unitCell != null)
                {
                    unit.SetCell(unitCell);
                    unitCell.Unit = unit;
                    Debug.Log($"Linked unit to cell at {unitCell.transform.position}");
                }
            }
        }
    }

    private Cell FindCellForUnit(Unit unit)
    {
        Vector3 unitPosition = unit.transform.position;
        foreach (Cell cell in cells)
        {
            if (Vector3.Distance(unitPosition, cell.transform.position) < 1f)
            {
                return cell;
            }
        }
        return null;
    }

    public void OnDestroy()
    {
        // Отписываемся от событий при уничтожении объекта
        foreach (Cell cell in cells)
        {
            cell.OnPointerClickEvent -= HandleCellClick;
        }
    }
}
