using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public event EventHandler OnHealthDepleted;
    private int _maxHealthValue;
    private int _currHealthValue;

    public void Init(int maxHealth) {
        _maxHealthValue = maxHealth;
    }

    public void Heal(int healValue) {
        _currHealthValue += healValue;
        SetHealthValidation();
    }

    public void Damage(int damageValue) {
        _currHealthValue -= damageValue;
        SetHealthValidation();
    }

    private void SetHealthValidation() {
        if (_currHealthValue > _maxHealthValue) _currHealthValue = _maxHealthValue;
        else if (_currHealthValue < 0) _currHealthValue = 0;

        if (_currHealthValue == 0) OnHealthDepleted?.Invoke(this, EventArgs.Empty);
    }
}