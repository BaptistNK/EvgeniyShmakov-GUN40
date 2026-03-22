using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellManager : MonoBehaviour
{

    [SerializeField] private string _unitTag = "Unit";
    [SerializeField] private string cellTag = "Cell";
    [SerializeField] private List<Cell> allCells = new List<Cell>();
    [SerializeField] private Cell[,] grid;
    [SerializeField] private float _cellSize = 1f;

    public event Action<Cell> OnCellClicked;

    public void SubscribeToCellClicks()
    {
        foreach (Cell cell in allCells)
        {
            cell.OnPointerClickEvent += HandleCellClick;
        }
    }

    public void UnsubscribeFromCellClicks()
    {
        foreach (Cell cell in allCells)
        {
            cell.OnPointerClickEvent -= HandleCellClick;
        }
    }

    private void HandleCellClick(Cell clickedCell)
    {
        OnCellClicked?.Invoke(clickedCell);
    }

    public void SetupNeighbourConnections()
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell currentCell = grid[x, y];
                if (currentCell == null) continue;
                SetupNeighbourForCell(currentCell, x, y, width, height);
            }
        }
    }

private void SetupNeighbourForCell(Cell cell, int x, int y, int width, int height)
    {
        var neighbourOffsets = new Dictionary<NeighbourType, (int dx, int dy)>
        {
            [NeighbourType.Top] = (0, 1),
            [NeighbourType.Right] = (1, 0),
            [NeighbourType.Bottom] = (0, -1),
            [NeighbourType.Left] = (-1, 0),
            [NeighbourType.TopRight] = (1, 1),
            [NeighbourType.TopLeft] = (-1, 1),
            [NeighbourType.BottomRight] = (1, -1),
            [NeighbourType.BottomLeft] = (-1, -1)
        };

        foreach(var neighbour in neighbourOffsets)
        {
            NeighbourType type = neighbour.Key;
            int newX = x + neighbour.Value.dx;
            int newY = y + neighbour.Value.dy;

            if (newX >= 0 && newX < width && newY >= 0 && newY < height)
            {
                Cell neighbourCell = grid[newX, newY];
                if(neighbourCell != null )
                {
                    cell.AddNeighbour(type, neighbourCell);
                }
            }
        }

    }

    public void AssignUnitsToCells()
    {
        GameObject[] unitObjects = GameObject.FindGameObjectsWithTag(_unitTag);
        List<Unit> units = new List<Unit>();

        foreach(GameObject obj in unitObjects)
        {
            Unit unit = obj.GetComponent<Unit>();
            if(unit!= null)
            {
                units.Add(unit);
            }
        }

        foreach (Unit unit in units)
        {
            Cell cell = FindCellByPosition(unit.transform.position);
        }

    }

    private Cell FindCellByPosition(Vector3 worldPosition)
    {
        int xIndex = Mathf.RoundToInt(worldPosition.x/_cellSize);
        int yIndex = Mathf.RoundToInt(worldPosition.y/_cellSize);

        if (xIndex >= 0 && xIndex < grid.GetLength(0) && yIndex >= 0 && yIndex < grid.GetLength(1)) 
        {
            return grid[xIndex, yIndex];
        }

        return null;
    }

    private void FindAllCellsInScene()
    {
        allCells.Clear();
        GameObject[] cellObjects = GameObject.FindGameObjectsWithTag(cellTag);
        foreach(GameObject obj in cellObjects)
        {
            Cell cell = obj.GetComponent<Cell>();
            if(cell != null)
            {
                allCells.Add(cell);
            }
            else
            {
                Debug.Log($"Объект {obj.name} имеет тег {cellTag}");
            }
        }
    }
    public List<Cell> GetAllCells()
    {
        return allCells;
    }
    // Start is called before the first frame update
    void Start()
    {        
        FindAllCellsInScene();
        SubscribeToCellClicks();
        Debug.Log($"Найдено клеток в сцене: {allCells.Count}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
