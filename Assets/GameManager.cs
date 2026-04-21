using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Настройка кеглей")]
    public List<Pin> allPins = new List<Pin>();
    [SerializeField] private int totalPins = 10;
    private int fallenPinsCount = 0;
    private HashSet<Pin> fallenPinsSet = new HashSet<Pin>();
    [SerializeField] private TextMeshProUGUI KnockedText;

    private void Awake()
    {
        Instance = this;
    }
    void InitializePins()
    {
        Pin[] pins = FindObjectsOfType<Pin>();
        allPins.Clear();
        foreach (Pin pin in pins)
        {
            allPins.Add(pin);
        }
        Debug.Log($"Найдено кеглей: {allPins.Count}");
    }

    public void RegisterFallenPin(Pin fallenPin)
    {
        if(!fallenPinsSet.Contains(fallenPin))
        {
            fallenPinsSet.Add(fallenPin);
            fallenPinsCount++;
            UpdateScoreDisplay();
        }
    }

    private void UpdateScoreDisplay()
    {
        Debug.Log($"Сбито кеглей: {fallenPinsCount} / {totalPins}");
        KnockedText.text = $"Score: {fallenPinsCount}";
    }

    public void ResetAllPins()
    {
        if(allPins==null)
        {
            Debug.LogError("Non Initialize");
            return;
        }

        fallenPinsCount = 0;
        fallenPinsSet.Clear();

        foreach (Pin pin in allPins)
        {
            pin.ResetPin();
        }

        UpdateScoreDisplay();
    }

    public int GetFallenPinCounts()
    {
        return fallenPinsCount;
    }
}
