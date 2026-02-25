using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private MeshRenderer _focusMesh;
    [SerializeField] private MeshRenderer _selectMesh;

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
        if(_selectMesh != null)
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

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
