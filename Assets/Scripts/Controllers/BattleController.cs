using UnityEngine;
using System.Collections.Generic;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; private set; }

    private Unit selectedUnit = null;
    private List<Cell> availableMoves = new List<Cell>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void HandleUnitClick(Unit unit)
    {
        if (PlayerController.Instance == null || PlayerController.Instance.IsInputLocked)
            return;

        if (selectedUnit != null)
        {
            selectedUnit.Deselect();
            ClearAvailableMoves();
        }

        selectedUnit = unit;
        selectedUnit.Select();
        availableMoves = Battlefield.Instance?.GetValidMoves(unit) ?? new List<Cell>();
        HighlightAvailableMoves();
    }

    public void HandleCellClick(Cell cell)
    {
        if (PlayerController.Instance == null || PlayerController.Instance.IsInputLocked)
            return;

        if (availableMoves.Contains(cell))
        {
            PlayerController.Instance.ExecuteMove(selectedUnit, cell);
            selectedUnit = null;
            ClearAvailableMoves();
        }
    }

    private void HighlightAvailableMoves()
    {
        foreach (var cell in availableMoves)
        {
            cell?.Highlight();
        }
    }

    private void ClearAvailableMoves()
    {
        foreach (var cell in availableMoves)
        {
            cell?.RemoveHighlight();
        }
        availableMoves.Clear();
    }
}
