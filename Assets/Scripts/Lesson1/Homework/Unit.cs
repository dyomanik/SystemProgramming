using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private int _health;
    private bool _isHealing;

    void Start()
    {
        ReceiveHealing();
    }

    public void ReceiveHealing()
    {
        if (_isHealing) return;
        StartCoroutine(HealingCoroutine());
    }

    IEnumerator HealingCoroutine()
    {
        _isHealing = true;
        float timer = 3;
        while (timer > 0 && _health < 100)
        {
            _health += 5;
            if (_health > 100)
            {
                _health = 100;
            }
            timer = timer - 0.5f;
            yield return new WaitForSecondsRealtime(0.5f); 
        }
        _isHealing = false;
    }
}
