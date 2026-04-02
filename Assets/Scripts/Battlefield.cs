using System.Collections.Generic;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    public static Battlefield Instance { get; private set; }

    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Unit unitPrefab;

    private Cell[,] cells;
    private Dictionary<Vector2Int, Unit> unitsOnBoard = new Dictionary<Vector2Int, Unit>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeBoard();
        SetupInitialUnits();
    }

    private void InitializeBoard()
    {
        cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                Cell cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.GridPosition = new Vector2Int(x, y);
                cells[x, y] = cell;
            }
        }
    }

    private void SetupInitialUnits()
    {
        // Расстановка белых шашек (нижние 3 ряда)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if ((x + y) % 2 == 1) // Только чёрные клетки
                {
                    SpawnUnit(x, y, true);
                }
            }
        }

        // Расстановка чёрных шашек (верхние 3 ряда)
        for (int x = 0; x < width; x++)
        {
            for (int y = height - 3; y < height; y++)
            {
                if ((x + y) % 2 == 1)
                {
                    SpawnUnit(x, y, false);
                }
            }
        }
    }

    private void SpawnUnit(int x, int y, bool isWhite)
    {
        Vector3 position = new Vector3(x, 0.5f, y); // Высота 0.5 для размещения на кубе
        Unit unit = Instantiate(unitPrefab, position, Quaternion.identity, transform);
        unit.Initialize(isWhite);
        cells[x, y].SetUnit(unit);
        unitsOnBoard[new Vector2Int(x, y)] = unit;
    }

    /// <summary>
    /// Получает список допустимых ходов для указанной фишки
    /// </summary>
    /// <param name="unit">Фишка, для которой ищем ходы</param>
    /// <returns>Список доступных клеток для хода</returns>
    public List<Cell> GetValidMoves(Unit unit)
    {
        List<Cell> validMoves = new List<Cell>();
        Vector2Int currentPos = unit.CurrentCell.GridPosition;
        int direction = unit.IsWhite ? 1 : -1; // Белые ходят вверх, чёрные вниз

        // Проверка диагональных ходов вперёд
        CheckMove(currentPos.x + 1, currentPos.y + direction, validMoves);
        CheckMove(currentPos.x - 1, currentPos.y + direction, validMoves);

        // Если фишка — дамка, добавляем ходы назад
        if (unit.IsKing)
        {
            CheckMove(currentPos.x + 1, currentPos.y - direction, validMoves);
            CheckMove(currentPos.x - 1, currentPos.y - direction, validMoves);
        }

        return validMoves;
    }

    /// <summary>
    /// Проверяет, можно ли сделать ход в указанную клетку
    /// </summary>
    /// <param name="x">Координата X</param>
    /// <param name="y">Координата Y</param>
    /// <param name="validMoves">Список допустимых ходов</param>
    private void CheckMove(int x, int y, List<Cell> validMoves)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            Cell targetCell = cells[x, y];
            if (targetCell != null && targetCell.UnitOnCell == null)
            {
                validMoves.Add(targetCell);
            }
        }
    }

    /// <summary>
    /// Получает клетку по координатам
    /// </summary>
    /// <param name="position">Вектор координат (x, y)</param>
    /// <returns>Клетка на поле или null, если координаты вне поля</returns>
    public Cell GetCellAt(Vector2Int position)
    {
        if (position.x >= 0 && position.x < width &&
            position.y >= 0 && position.y < height)
        {
            return cells[position.x, position.y];
        }
        return null;
    }

    /// <summary>
    /// Удаляет фишку с поля
    /// </summary>
    /// <param name="unit">Удаляемая фишка</param>
    public void RemoveUnit(Unit unit)
    {
        if (unit.CurrentCell != null)
        {
            unit.CurrentCell.SetUnit(null);
        }
        Vector2Int pos = unit.CurrentCell?.GridPosition ?? Vector2Int.zero;
        unitsOnBoard.Remove(pos);
        Destroy(unit.gameObject);
    }

    /// <summary>
    /// Перемещает фишку на новую клетку
    /// </summary>
    /// <param name="unit">Перемещаемая фишка</param>
    /// <param name="targetCell">Целевая клетка</param>
    public void MoveUnitToCell(Unit unit, Cell targetCell)
    {
        Vector2Int oldPos = unit.CurrentCell.GridPosition;
        unitsOnBoard.Remove(oldPos);
        unit.CurrentCell.SetUnit(null);
        targetCell.SetUnit(unit);
        unitsOnBoard[targetCell.GridPosition] = unit;
    }
}
