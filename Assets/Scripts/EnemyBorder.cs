using UnityEngine;
using System.Collections;

public class EnemyBorder : MonoBehaviour {
    public void OnTriggerEnter2D(Collider2D other) {
        other.SendMessage("BorderEntered", this.transform, SendMessageOptions.DontRequireReceiver);
    }
}
