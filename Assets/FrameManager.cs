using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameManager : MonoBehaviour
{

    [SerializeField] private int totalFrames = 10;
    [SerializeField] private Ball ball;
    [SerializeField] private TextMeshProUGUI currentFrameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private GameObject pinPrefab;          // Префаб кегли
    [SerializeField] private Transform pinsSpawnPoint;       // Точка спавна 
    [SerializeField] private List<GameObject> currentPins = new List<GameObject>(); // Список созданных кеглей
    [SerializeField] private bool isSpawnPointSet = false;    // Флаг: точка спавна установлена?
    [SerializeField] private Vector3 customSpawnPoint;         // Координаты выбранной точки спавна
    [SerializeField] private GameObject spawnPointIndicator;     // Визуальный индикатор точки спавна (опционально)

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
        SpawnPins();
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
        Debug.Log($"Начинаем фрейм {currentFrame}. Сброс кеглей...");
        GameManager.Instance.ResetAllPins();

        isSpawnPointSet = false;
        customSpawnPoint = Vector3.zero;

        SpawnPins();
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
    private void SetSpawnPointFromClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Проверяем, попал ли луч в коллайдер с тегом "floor"
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,
            LayerMask.GetMask("floor"))) 
        {
            customSpawnPoint = hit.point;
            customSpawnPoint.y += 0.8f;
            isSpawnPointSet = true;

            // Показываем индикатор точки спавна (если назначен префаб)
            if (spawnPointIndicator != null)
            {
                GameObject indicator = Instantiate(spawnPointIndicator, customSpawnPoint, Quaternion.identity);
                // Можно добавить таймер уничтожения индикатора через 3 секунды
                Destroy(indicator, 3f);
            }

            Debug.Log($"Точка спавна шара установлена: {customSpawnPoint}");
            SpawnNewBall();
            UpdateStatusText("Точка спавна шара установлена. Готов к броску.");
        }
        else
        {
            UpdateStatusText("Кликните на дорожке, чтобы установить точку спавна шара.");
            
        }
    }
    private void SpawnNewBall()
    {
        Vector3 spawnPosition;
        // Если точка спавна установлена кликом — используем её
        if (isSpawnPointSet)
        {
            spawnPosition = customSpawnPoint;
        }
        // Иначе используем точку из инспектора (если назначена)
        /*else if (ballSpawnPoint != null)
        {
            spawnPosition = ballSpawnPoint.position;
        }*/
        else
        {
            Destroy(currentBall);
            return;
        }
        if (ballSpawnPoint == null && !isSpawnPointSet)
        {
            Debug.LogError("ballSpawnPoint не назначен в инспекторе, и точка спавна не установлена кликом!");
            return;
        }

        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
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
            // Если точка спавна ещё не установлена, ждём клика
            if (!isSpawnPointSet)
            {
                UpdateStatusText("Кликните на дорожке, чтобы установить точку спавна шара.");
                return;
            }
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

    void Update()
    {
        // Если точка спавна ещё не установлена и игрок кликнул левой кнопкой мыши
        if (!isSpawnPointSet && Input.GetMouseButtonDown(0))
        {
            SetSpawnPointFromClick();
        }
    }


    public void RegisterThrow(int pinsKnocked)
    {
        Debug.Log($"RegisterThrow вызван. Сбито: {pinsKnocked}, текущий бросок: {currentThrow}, фрейм: {currentFrame}");

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
    private void SpawnPins()
    {
        // Удаляем старые кегли, если они есть
        foreach (var pin in currentPins)
        {
            if (pin != null)
                Destroy(pin);
        }
        currentPins.Clear();

        // Проверяем, назначена ли точка спавна
        if (pinsSpawnPoint == null)
        {
            Debug.LogError("pinsSpawnPoint не назначен в инспекторе!");
            return;
        }

        // Создаём 10 кеглей (стандартный набор)
        for (int i = 0; i < 10; i++)
        {
            // Рассчитываем позицию для каждой кегли (треугольная расстановка)
            Vector3 spawnPosition = CalculatePinPosition(i, pinsSpawnPoint.position);
            GameObject newPin = Instantiate(pinPrefab, spawnPosition, Quaternion.identity);
            currentPins.Add(newPin);
        }
    }

    private Vector3 CalculatePinPosition(int pinIndex, Vector3 basePosition)
    {
        float spacing = 0.7f; // Расстояние между кеглями

        switch (pinIndex)
        {
            case 0: return basePosition + new Vector3(0, 0, 0);
            case 1: return basePosition + new Vector3(-spacing, 0, spacing);
            case 2: return basePosition + new Vector3(spacing, 0, spacing);
            case 3: return basePosition + new Vector3(-2 * spacing, 0, 2 * spacing);
            case 4: return basePosition + new Vector3(0, 0, 2 * spacing);
            case 5: return basePosition + new Vector3(2 * spacing, 0, 2 * spacing);
            case 6: return basePosition + new Vector3(-3 * spacing, 0, 3 * spacing);
            case 7: return basePosition + new Vector3(-spacing, 0, 3 * spacing);
            case 8: return basePosition + new Vector3(spacing, 0, 3 * spacing);
            case 9: return basePosition + new Vector3(3 * spacing, 0, 3 * spacing);
            default: return basePosition;
        }
    }

    public int GetCurrentFrame() => currentFrame;
    public int GetCurrentThrow() => currentThrow;
    public bool IsGameOver() => currentFrame > totalFrames;
}
