using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<Cell> OnPointerClickEvent;
    [SerializeField] private MeshRenderer _focus;
    [SerializeField] private MeshRenderer _select;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClickEvent.Invoke(this);
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
            _select.enabled = true;
            GetComponent<Renderer>().material = material;
    }

    public void ResetSelect()
    {
        _select.enabled = false;
    }
}
