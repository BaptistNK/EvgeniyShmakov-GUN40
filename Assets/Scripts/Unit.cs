using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private Cell _currentCell;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private Cell cell;
    public event System.Action OnMoveEndCallback;
    private bool _isMoving = false;

    public Cell Cell
    {
        get { return cell; }
        private set { cell = value; }
    }

    public void SetCell(Cell newCell)
    {
        // Отвязываем от старой клетки, если была
        if (cell != null && cell.Unit == this)
        {
            cell.Unit = null;
        }

        cell = newCell;

        // Привязываем к новой клетке
        if (newCell != null)
        {
            newCell.Unit = this;
            // Перемещаем юнита на позицию клетки (чуть выше поверхности)
            transform.position = newCell.transform.position + Vector3.up * 0.5f;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentCell != null)
            _currentCell.OnPointerClick(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_currentCell != null)
            _currentCell.OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_currentCell != null)
            _currentCell.OnPointerExit(eventData);
    }

    public void Move(Cell cell)
    {
        if (_isMoving || cell == null || _currentCell == cell)
            return;
        _isMoving = true;
        StartCoroutine(MoveToCellRoutine(cell));
    }

    private IEnumerator MoveToCellRoutine(Cell targetCell)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetCell.transform.position;
        float elapsedTime = 0f;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / _moveSpeed;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
        }
        yield return null;
        transform.position = targetPosition;
        _currentCell = targetCell;
        _isMoving = false;
        OnMoveEndCallback?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
