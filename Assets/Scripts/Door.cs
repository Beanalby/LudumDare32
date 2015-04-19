using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    private float distScale = 1f;

    private Vector3 basePos;
    private Rigidbody2D rb;

    public void Start() {
        basePos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetScaleDelta(float dist) {
        Vector3 pos = basePos;
        pos.y += dist * distScale;
        rb.MovePosition(pos);
    }
}
