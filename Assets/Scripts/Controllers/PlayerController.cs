using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float moveAnimationDuration = 0.5f;
    public bool IsInputLocked { get; private set; } = false;

    void Awake()
    {
        Instance = this;
    }

    public void ExecuteMove(Unit unit, Cell targetCell)
    {
        StartCoroutine(MoveUnitCoroutine(unit, targetCell));
    }

    private IEnumerator MoveUnitCoroutine(Unit unit, Cell targetCell)
    {
        IsInputLocked = true;

        // Сохраняем старую позицию
        Vector2Int oldPosition = unit.CurrentCell.GridPosition;
        Vector3 startPosition = unit.transform.position;
        Vector3 endPosition = targetCell.transform.position + new Vector3(0, 0.5f, 0);

        // Удаляем фишку со старой клетки
        unit.CurrentCell.SetUnit(null);

        float elapsedTime = 0f;
        while (elapsedTime < moveAnimationDuration)
        {
            unit.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Устанавливаем фишку на новую клетку
        unit.transform.position = endPosition;
        targetCell.SetUnit(unit);

        // Проверяем, стала ли шашка дамкой
        CheckForKingPromotion(unit, targetCell.GridPosition);

        IsInputLocked = false;
        CheckGameEnd();
    }

    private void CheckForKingPromotion(Unit unit, Vector2Int position)
    {
        if (unit.IsWhite && position.y == 7) // Белые достигают последнего ряда
        {
            PromoteToKing(unit);
        }
        else if (!unit.IsWhite && position.y == 0) // Чёрные достигают последнего ряда
        {
            PromoteToKing(unit);
        }
    }

    private void PromoteToKing(Unit unit)
    {
        unit.IsKing = true;
        // Здесь можно добавить визуальные эффекты для превращения в дамку
        Debug.Log("Unit promoted to king!");
    }

    private void CheckGameEnd()
    {
        // Логика проверки окончания игры (победа/поражение)
        // Можно проверить количество оставшихся фишек у каждого игрока
        // и объявить победителя
    }
}
