using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FrameManager : MonoBehaviour
{

    [SerializeField] private int totalFrames = 10;
    [SerializeField] private Ball ball;
    [SerializeField] private TextMeshProUGUI currentFrameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPoint;
    private GameObject currentBall;

    private int currentFrame = 1;
    private int currentThrow = 1;
    private bool isTenthFrame = false;
    private bool extraThrowInTenth = false;

    private List<int> throws = new List<int>();
    private List<int> frameScore = new List<int>();
    private int totalScore = 0; 

    void Start()
    {
        SpawnNewBall();
        UpdateUIFrame();
        UpdateUIScore();
        SetInitialFrameStatus();
    }

    private void UpdateStatusText(string message = "")
    {
        if (statusText != null)
        {
            if (string.IsNullOrEmpty(message))
            {
                if (currentThrow == 1)
                {
                    statusText.text = "Первый бросок фрейма";
                }
                else if (currentThrow == 2)
                {
                    statusText.text = "Второй бросок фрейма";
                }
                else
                {
                    statusText.text = $"Дополнительный бросок";
                }
            }
            else
            {
                statusText.text = message;
            }
        }
    }

    void StartNewFrame()
    {
        if (currentFrame >= totalFrames)
        {
            UpdateStatusText("Игра завершена! Финальный счёт: " + totalScore);
            return;
        }

        SpawnNewBall();

        currentFrame++;
        currentThrow = 1;
        isTenthFrame = (currentFrame == 10);
        extraThrowInTenth = false;

        UpdateUIFrame();
        SetInitialFrameStatus();
    }


    public bool IsLastFrame() => currentFrame== totalFrames;

    public bool CanMakeThrow()
    {
        if(isTenthFrame)
        {
            return currentThrow <= (extraThrowInTenth ? 3 : 2);
        }
        else
        {
            return currentThrow <= 2;
        }
    }

    public void SpawnNewBall()
    {
        if (ballSpawnPoint == null)
        {
            Debug.LogError("ballSpawnPoint не назначен в инспекторе!");
            return;
        }

        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
        ball = currentBall.GetComponent<Ball>();

        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            ball._rb = rb;
        }

        ball.ResetForNextThrow(currentThrow);
    }




    private void ProceedToNextThrow()
    {
        currentThrow++;

        if (CanMakeThrow())
        {
            SpawnNewBall();
            UpdateStatusText($"Бросок {currentThrow}. Готов к броску.");
        }
        else
        {
            if (!IsGameOver())
            {
                StartNewFrame();
            }
            else
            {
                UpdateStatusText("Игра завершена! Финальный счёт: " + totalScore);
            }
        }

        UpdateUIFrame();
        UpdateUIScore();
    }



    public void RegisterThrow(int pinsKnocked)
    {
        throws.Add(pinsKnocked);
        CalculateScore(pinsKnocked);

        string statusMessage = DetermineFrameStatus(pinsKnocked);
        UpdateStatusText(statusMessage);
        if (currentThrow == 1 && pinsKnocked == 10 && !isTenthFrame)
        {
            StartNewFrame();
            return;
        }

        if (isTenthFrame)
        {
            if (pinsKnocked == 10)
            {
                extraThrowInTenth = true;
            }
            else if (currentThrow == 2 && GetFramePinsSum() == 10)
            {
                extraThrowInTenth = true;
            }
        }
        ProceedToNextThrow();
    }

    string DetermineFrameStatus(int pinsKnocked)
    {
        if (isTenthFrame)
        {
            if (currentThrow == 1 && pinsKnocked == 10)
            {
                return "10‑й фрейм: Страйк! Дополнительный бросок.";
            }
            else if (currentThrow == 2 && (pinsKnocked == 10 || GetFramePinsSum() == 10))
            {
                return "10‑й фрейм: Спэр или страйк! Последний бросок.";
            }
            else if (currentThrow == 3)
            {
                return "10‑й фрейм: Последний бросок. Фрейм завершится после этого броска.";
            }
            else
            {
                return $"10‑й фрейм, бросок {currentThrow}. Продолжайте.";
            }
        }
        else
        {
            if (currentThrow == 1 && pinsKnocked == 10)
            {
                return "Страйк! Переход к следующему фрейму";
            }
            else if (currentThrow == 2)
            {
                int frameSum = GetFramePinsSum();
                if (frameSum == 10)
                {
                    return "Спэр! Фрейм завершён.";
                }
                else
                {
                    return "Фрейм завершён (открытый).";
                }
            }
            else
            {
                return $"Бросок {currentThrow}. Готов к броску.";
            }
        }
    }


    int GetFramePinsSum()
    {
        int sum = 0;
        int throwCount = throws.Count;
        if (throwCount == 0) return 0;
        int startIndex = Mathf.Max(0, throwCount - 2);

        for (int i = startIndex; i < throwCount; i++) 
        {
            sum += throws[i];
        }
        return sum;
    }
    void UpdateUIFrame()
    {
        if(currentFrameText!=null)
        {
            currentFrameText.text = $"Фрейм: {currentFrame}/{totalFrames}";
        }
    }

    void UpdateUIScore()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Общий счет: {totalScore}";
        }
    }

    private void CalculateScore(int pinsKnocked)
    {
        totalScore += pinsKnocked;
    }

    public bool IsFrameComplete()
    {
        if (currentFrame > totalFrames) return true;

        if (isTenthFrame)
        {
            return currentThrow > (extraThrowInTenth ? 3 : 2);
        }
        else
        {
            if (currentThrow >= 2) return true;
            if (throws.Count > 0 && throws[throws.Count - 1] == 10) return true;
            return false;
        }
    }


    private void SetInitialFrameStatus()
    {
        if (currentThrow == 1)
        {
            UpdateStatusText("Готов к первому броску фрейма");
        }
        else if (currentThrow == 2)
        {
            UpdateStatusText("Готов ко второму броску фрейма");
        }
        else
        {
            UpdateStatusText("Готов к дополнительному броску в 10‑м фрейме");
        }
    }

    public int GetCurrentFrame() => currentFrame;
    public int GetCurrentThrow() => currentThrow;
    public bool IsGameOver() => currentFrame > totalFrames;
}
