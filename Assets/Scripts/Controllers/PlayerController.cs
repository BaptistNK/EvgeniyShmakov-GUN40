using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputBlock _inputBlock;
    [SerializeField] private Unit _unit;

    /*public IEnumerator Move(Unit unit, Cell cell)
    {
        _inputBlock.SetInputBlocked(true);
        yield return _unit.Move(unit, cell);
    }*/
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
