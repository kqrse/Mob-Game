using UnityEngine;

public class Footprint : MonoBehaviour {
    private CircleCollider2D _footprint;

    private void Awake() {
        _footprint = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    private void Update() {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public CircleCollider2D GetFootprintCollider() {
        if (_footprint == null) Debug.LogError("NO FOOTPRINT!?");
        return _footprint;
    }
}