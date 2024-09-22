using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        OnGameStart ??= new UnityEvent();
    }
    #endregion

    [field: SerializeField]
    public bool IsGameStarted { get; set; }
    PlayableDirector _dir;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI _startText;
    [SerializeField] TextMeshProUGUI _timerText;
    AudioSource _audioSource;

    public UnityEvent OnGameStart;

    public Queue<Fan> Fans { get; set; } = new();
    [SerializeField] int _maxFans = 3;

    private void Start()
    {
        _dir = GetComponent<PlayableDirector>();
        _audioSource = GetComponent<AudioSource>();
        _timerText.alpha = 0;
    }

    public void StartGame()
    {
        IsGameStarted = true;
        _timerText.DOFade(1, 0.5f);
        OnGameStart.Invoke();
    }

    void Update()
    {
        if (!IsGameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startText.DOFade(0, 0.3f);
                if (_dir != null)
                    _dir.Play();
                _audioSource.Play();
            }
        }
    }

    public void AddFanToConcurrency(Fan fan)
    {
        if (Fans.Count >= _maxFans)
        {
            var deqFan = Fans.Dequeue();
            deqFan.ToggleOn(state: false, force: true);
        }

        Fans.Enqueue(fan);
    }
}
