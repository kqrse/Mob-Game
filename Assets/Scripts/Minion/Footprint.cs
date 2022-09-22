using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Footprint : MonoBehaviour {
    private CircleCollider2D _footprint;

    private void Awake() {
        _footprint = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    private void Update() {
    }

    public CircleCollider2D GetFootprintCollider() {
        if (_footprint == null) Debug.LogError("NO FOOTPRINT!?");
        return _footprint;
    }
}