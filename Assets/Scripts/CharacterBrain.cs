using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBrain : MonoBehaviour
{
    public Transform basePoint;
    private Animator anim;
    public int itemsCollected = 0;
    public int maxCapacity = 5;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Collected()
    {
        itemsCollected++;
        if(itemsCollected>=maxCapacity)
        {
            GetComponent<Animator>().SetBool("IsFull", true);
        }
    }
    public void FinishCollection()
    {
        itemsCollected++;
        Debug.Log($"Собрано: {itemsCollected}/{maxCapacity}");

        if (itemsCollected >= maxCapacity)
        {
            GetComponent<Animator>().SetBool("IsFull", true);
        }

        GetComponent<Animator>().SetTrigger("CollectionFinished");
    }

    public void UnloadItems()
    {
        itemsCollected = 0;
        Debug.Log("Предметы выгружены на базе.");
    }
}
