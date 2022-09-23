using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    private Health _healthObj;
    [SerializeField] private Image healthBarFill;
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

        Assert.IsNotNull(_healthObj);
        Assert.IsNotNull(healthBarFill);
        Assert.IsNotNull(_followUnitTransform);
        Assert.IsNotNull(_cam);
        Assert.IsNotNull(_canvasTransform);

        transform.SetParent(_canvasTransform);
        _isInitialized = true;
    }

    private void Update() {
        if (!_isInitialized) return;

        var fillPercentage = 1 - _healthObj.GetCurrentHealthPercentage();
        healthBarFill.fillAmount = fillPercentage;
        Vector2 screenPos = _cam.WorldToScreenPoint(_followUnitTransform.position);
        transform.position = new Vector3(screenPos.x, screenPos.y + _offsetY, 1);
    }
}