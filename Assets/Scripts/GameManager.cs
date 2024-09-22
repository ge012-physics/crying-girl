using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Start,
    Game,
    End
}

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

    public GameState CurrentState { get; set; }

    [field: SerializeField]
    public bool IsGameStarted { get; set; }
    PlayableDirector _dir;

    [Header("UI")]
    [SerializeField] Transform _startText;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] Transform _endChalkboard;
    [SerializeField] Image _fade;
    [SerializeField] TextMeshProUGUI _endTimer, _endTear;
    AudioSource _audioSource;

    [Header("References")]
    [SerializeField] GameClock _clock;
    [SerializeField] TearSpawner _tearSpawner;


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
        if (!IsGameStarted && CurrentState == GameState.Start)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CurrentState = GameState.Game;
                _startText.DOLocalMoveY(1200, 1f).OnComplete(() =>
                {
                    if (_dir != null)
                        _dir.Play();
                    _audioSource.Play();
                }).SetEase(Ease.OutBounce);
            }
        }
    }

    public void GameOver()
    {
        print("GAME OVER!");
        IsGameStarted = false;
        CurrentState = GameState.End;
        _fade.gameObject.SetActive(true);
        _endTimer.text = Mathf.Floor(_clock.TimeElapsed).ToString() + " seconds";
        _endTear.text = _tearSpawner.TearCount.ToString() + " times";

        var seq = DOTween.Sequence();
        seq.Append(_fade.DOFade(1, 1))
            .Append(_endChalkboard.DOLocalMoveY(50, 2).SetEase(Ease.OutBounce));

        seq.Play();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddFanToConcurrency(Fan fan)
    {
        // (zsfer): I HATE THIS !!!!! I WANT TO IMPLEMENT MY OWN LINKED LIST !!!
        if (Fans.Contains(fan))
        {
            var queue = new Queue<Fan>(Fans.Where(f => f != fan));
            Fans = queue;
            return;
        }

        if (Fans.Count >= _maxFans)
        {
            var deqFan = Fans.Dequeue();
            deqFan.ToggleOn(state: false, force: true);
        }

        Fans.Enqueue(fan);
    }
}
