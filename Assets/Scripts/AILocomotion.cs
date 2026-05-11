using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AILocomotion : MonoBehaviour
{
    [Header("Настройки пути")]
    [SerializeField] private List<Transform> _waypoints; // Точки маршрута
    [SerializeField] private float _duration = 8f;       // Длительность всего пути
    [SerializeField] private Ease _easeType = Ease.Linear;

    [Header("Визуальные эффекты")]
    [SerializeField] private Vector3 _punchScaleAmount = new Vector3(0.1f, 0.2f, 0.1f);
    [SerializeField] private ParticleSystem _dustParticles;

    //private Color _initialColor;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if (_waypoints != null && _waypoints.Count > 0)
        {
            AnimateCharacter3D();
        }        
    }

    private void AnimateCharacter3D()
    {
        Vector3[] pathArray = _waypoints.Select(wp => wp.position).ToArray();

        // Создаем последовательность анимаций
        Sequence mainSequence = DOTween.Sequence();

        // Анимация движения
        mainSequence.Append(transform.DOPath(pathArray, _duration, PathType.CatmullRom)
            .SetEase(_easeType)
            .SetLookAt(0.01f, Vector3.forward, Vector3.up) 
            .OnStart(() => {
                if (_animator != null) _animator.SetFloat("Speed", 1f);
                if (_dustParticles != null) _dustParticles.Play();
            })
            .OnComplete(() => {
                if (_animator != null) _animator.SetFloat("Speed", 0f);
                if (_dustParticles != null) _dustParticles.Stop();
            }));

        // DOPunchScale колебания размера
        mainSequence.Join(transform.DOPunchScale(_punchScaleAmount, 1.5f, 4, 0.5f).SetLoops(-1));

        // Настройка параметров всей последовательности
        mainSequence.SetLoops(-1, LoopType.Restart) // Повторять бесконечно
                    .SetDelay(1f);                  // Задержка перед началом
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
