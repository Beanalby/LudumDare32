using UnityEngine;
using System.Collections;

public class Signal : MonoBehaviour {
    SpriteRenderer sr;
    private float pulseDuration = 1f;
    private float pulseStart;

    private bool doLoop = true;

    Color color = Color.white;
    public void Start() {
        pulseStart = Time.time;
        sr = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        float percent = (Time.time - pulseStart) / pulseDuration;
        if (percent >= 1) {
            if (doLoop) {
                pulseStart = pulseStart + pulseDuration;
                percent = percent % 1;
            } else {
                Destroy(gameObject);
                return;
            }
        }
        float scale = 2 * percent;
        transform.localScale = new Vector3(scale, scale, scale);

        if (percent < .5f) {
            color.a = 1;
        } else if (percent < .75f) {
            color.a = 1 - ((percent - .5f) * 4);
        } else {
            color.a = 0;
        }
        sr.color = color;
    }

    public void Deactivate() {
        doLoop = false;
    }
}
