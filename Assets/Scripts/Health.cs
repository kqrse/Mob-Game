using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public event EventHandler OnHealthDepleted;
    [SerializeField] private float _maxHealthValue;
    [SerializeField] private float _currHealthValue;

    public void Init(float maxHealth) {
        _maxHealthValue = maxHealth;
        _currHealthValue = _maxHealthValue;
    }

    public void Heal(float healValue) {
        _currHealthValue += healValue;
        SetHealthValidation();
    }

    public void Damage(float damageValue) {
        _currHealthValue -= damageValue;
        SetHealthValidation();
    }

    public float GetCurrentHealthPercentage() {
        return _currHealthValue / _maxHealthValue;
    }

    private void SetHealthValidation() {
        if (_currHealthValue > _maxHealthValue) _currHealthValue = _maxHealthValue;
        else if (_currHealthValue < 0) _currHealthValue = 0;

        if (_currHealthValue == 0) OnHealthDepleted?.Invoke(this, EventArgs.Empty);
    }
}