using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    private Health _healthObj;
    [SerializeField] private Image healthBarFill;
    private Image _healthBar;
    private bool _isInitialized;
    public Transform _followUnitTransform;
    private Transform _canvasTransform;
    public float _offsetY;
    private Camera _cam;

    public void Init(Transform followUnitTransform, Health healthObj, float offsetY) {
        _followUnitTransform = followUnitTransform;
        _healthObj = healthObj;
        _offsetY = offsetY;
        _cam = Camera.main;
        _canvasTransform = GameObject.FindGameObjectWithTag("Canvas").transform;
        _healthBar = GetComponent<Image>();

        Assert.IsNotNull(_healthObj);
        Assert.IsNotNull(_healthBar);
        Assert.IsNotNull(healthBarFill);
        Assert.IsNotNull(_followUnitTransform);
        Assert.IsNotNull(_cam);
        Assert.IsNotNull(_canvasTransform);

        transform.SetParent(_canvasTransform);
        _isInitialized = true;

        healthBarFill.enabled = false;
        _healthBar.enabled = false;
    }

    private void Update() {
        if (!_isInitialized) return;

        var fillPercentage = 1 - _healthObj.GetCurrentHealthPercentage();
        healthBarFill.fillAmount = fillPercentage;

        // var color = healthBarFill.color;
        if (fillPercentage > 0) {
            Debug.Log("LETS GO");
            _healthBar.enabled = true;
            healthBarFill.enabled = true;
        }
        else {
            Debug.Log(fillPercentage);
        }

        Vector2 screenPos = _cam.WorldToScreenPoint(_followUnitTransform.position);
        transform.position = new Vector3(screenPos.x, screenPos.y + _offsetY, 1);
    }

    public void SetOpacity(float value) {
        var healthBarColor = _healthBar.color;
        _healthBar.color = new Color(healthBarColor.r, healthBarColor.g, healthBarColor.b, value);

        var healthBarFillColor = healthBarFill.color;
        healthBarFill.color = new Color(healthBarFillColor.r, healthBarFillColor.g, healthBarFillColor.b, value);
    }
}