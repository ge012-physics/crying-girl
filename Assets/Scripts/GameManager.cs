using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    #endregion

    [field: SerializeField]
    public bool IsGameStarted { get; set; }
    PlayableDirector _dir;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI _startText;

    private void Start()
    {
        _dir = GetComponent<PlayableDirector>();
    }

    public void StartGame()
    {
        IsGameStarted = true;
    }

    void Update()
    {
        if (!IsGameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startText.DOFade(0, 0.3f);
                _dir.Play();
            }
        }
    }
}
