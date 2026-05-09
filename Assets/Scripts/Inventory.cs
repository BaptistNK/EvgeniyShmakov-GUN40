using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int CollectionCount {  get; private set; }
    public int capacity = 3;

    public void AddItem()=> CollectionCount++;    
    public bool IsFull() => CollectionCount >= capacity;
    public void Clear() => CollectionCount = 0;
}
