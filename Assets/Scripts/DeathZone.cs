using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {
    public void OnTriggerEnter2D(Collider2D other) {
        other.SendMessage("Die");
    }
}
