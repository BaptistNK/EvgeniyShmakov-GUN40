using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneController : MonoInstaller
{
    public void OpenMainScene()
    {
        throw new NotImplementedException();
        SceneManager.LoadScene(0);
    }

    public void OpenGameScene()
    {
        SceneManager.LoadScene(0);
    }
}
