using UnityEngine;

public static class VectorHelper {
    public static Vector2 GetVectorToPoint(Vector3 source, Vector3 destination) {
        var source2D = new Vector2(source.x, source.y);
        var destination2D = new Vector2(destination.x, destination.y);

        return (destination2D - source2D).normalized;
    }

    public static Vector2 RotationToVector(float degrees) {
        var rotation = Quaternion.Euler(0, 0, degrees);
        Vector2 v = rotation * Vector3.down;

        return v;
    }
}