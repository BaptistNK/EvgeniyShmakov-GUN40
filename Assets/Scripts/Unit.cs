using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private Cell _currentCell;
    [SerializeField] private float _moveSpeed = 2f;
    public event System.Action OnMoveEndCallback;
    private bool _isMoving = false;

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
