using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperHealth : MonoBehaviour
{
    [SerializeField] private int _maxHP = 5;
    private int _currHP;

    void Start () 
    {
        _currHP = _maxHP;
    }

    public void TakeDamage(int damage) 
    {
        _currHP -= damage;
        Debug.Log("Paper HP: " + _currHP);

        if (_currHP <= 0) 
        {
            DestroyPaper();
        }
    }

    private void DestroyPaper() 
    {
        Debug.Log("Paper Destroyed!");
        Destroy(gameObject);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Paper HP: " + _currHP);
    }
}
