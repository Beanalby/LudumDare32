using UnityEngine;
using System.Collections;

public class Trapdoor : MonoBehaviour {
    private float openDistance = 1f;

    private Rigidbody2D rb;

    public void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetScaleDelta(float dist) {
        Debug.Log("dist=" + dist);
        dist = Mathf.Min(openDistance, dist);
        float percent = dist / openDistance;
        rb.MoveRotation(-90 * percent);
    }
}
