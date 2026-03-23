using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBlock : MonoBehaviour
{
    private bool isInputBlocked = false;
    public bool IsInputBlocked => isInputBlocked;
    public void SetInputBlocked(bool blocked)
    {
        isInputBlocked = blocked;
    }
}
