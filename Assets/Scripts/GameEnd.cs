using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public class GameEnd : MonoBehaviour {

        private bool IsEnded = false;
        public GUISkin skin;

        public void OnTriggerEnter2D(Collider2D other) {
            IsEnded = true;
            Player player = other.GetComponent<Player>();
            player.enabled = false;
        }

        public void OnGUI() {
            if (IsEnded) {
                GUI.skin = skin;
                GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Contgradulations, you escaped!");
            }
        }
    }
}