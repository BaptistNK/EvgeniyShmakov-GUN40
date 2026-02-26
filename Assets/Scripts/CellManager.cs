using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public static CellManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void OnCellClicked(Vector3 cellPosition)
    {
        Debug.Log("Клетка нажата: " + cellPosition);
    }
}
