using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;
    GameManager _game;
    float _timeElapsed;

    void Start()
    {
        _game = GameManager.Instance;
    }

    void Update()
    {
        if (!_game.IsGameStarted) return;

        _timeElapsed += Time.deltaTime;
        _timerText.text = Mathf.Floor(_timeElapsed).ToString();
    }
}
