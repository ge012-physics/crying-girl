using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager _game;
    void Start()
    {
        _game = GameManager.Instance;
    }

    void Update()
    {
        if (!_game.IsGameStarted) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Fan fan))
                    fan.ToggleOn();
            }
        }
    }
}
