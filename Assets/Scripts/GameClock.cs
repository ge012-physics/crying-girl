using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameClock : MonoBehaviour
{


    [SerializeField] TextMeshProUGUI _timerText;
    GameManager _game;
    public float TimeElapsed;

    void Start()
    {
        _game = GameManager.Instance;
    }

    void Update()
    {
        if (!_game.IsGameStarted) return;

        TimeElapsed += Time.deltaTime;
        _timerText.text = Mathf.Floor(TimeElapsed).ToString();
    }
}
