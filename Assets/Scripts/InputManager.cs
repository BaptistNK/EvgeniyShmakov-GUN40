using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference restartAction;
    [SerializeField] private GameObject restartUI;
    [SerializeField] private Image fillImage;
    [SerializeField] private float fillDuration = 2f;

    private Coroutine fillCoroutine;
    private bool isInputActive = false;
    private bool hasCompletedFill = false;

    void Start()
    {
        if (restartUI != null)
        {
            restartUI.SetActive(false);
        }

        if (restartAction != null)
        {
            restartAction.action.performed += OnRestartInputStarted;
            restartAction.action.canceled += OnRestartInputEnded;
        }

    }

    private void OnDestroy()
    {
        if (restartAction != null)
        {
            restartAction.action.performed -= OnRestartInputStarted;
            restartAction.action.canceled -= OnRestartInputEnded;
        }
    }
    private void OnRestartInputStarted(InputAction.CallbackContext context)
    {
        isInputActive = true;
        hasCompletedFill = false;

        if (restartUI != null)
            restartUI.SetActive(true);

        if (fillCoroutine == null)
        {
            fillCoroutine = StartCoroutine(FillRestartBarCoroutine());
        }
    }

    private void OnRestartInputEnded(InputAction.CallbackContext context)
    {
        isInputActive = false;

        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }

        if (!hasCompletedFill && restartUI != null)
        {
            restartUI.SetActive(false);
        }
    }

    private IEnumerator FillRestartBarCoroutine()
    {
        float elapsedTime = 0f;

        if (fillImage != null)
            fillImage.fillAmount = 0f;

        while (elapsedTime < fillDuration)
        {
            if (!isInputActive)
            {
                yield break;
            }

            elapsedTime += Time.deltaTime;
            float fillProgress = Mathf.Clamp01(elapsedTime / fillDuration);

            if (fillImage != null)
                fillImage.fillAmount = fillProgress;

            yield return null; 
        }

        hasCompletedFill = true;

        if (fillImage != null)
            fillImage.fillAmount = 1f;

        RestartScene();

        if (restartUI != null)
            restartUI.SetActive(false);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
