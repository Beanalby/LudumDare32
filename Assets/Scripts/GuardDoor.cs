using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LudumDare32 {
    public class GuardDoor : MonoBehaviour {
        public List<Mover> enemies;
        public Signal signal;

        private float moveStart = -1, moveDuration = 1f, moveDistance = 3f;
        private Vector3 basePos;

        public void Start() {
            basePos = transform.position;
            foreach (Mover m in enemies) {
                m.OnDeath += OnDeath;
            }
        }

        public void FixedUpdate() {
            if (moveStart != -1) {
                float percent = (Time.time - moveStart) / moveDuration;
                Vector3 pos = basePos;
                if (percent >= 1) {
                    pos.y += moveDistance;
                    moveStart = -1;
                } else {
                    pos.y += moveDistance * percent;
                }
                transform.position = pos;
            }
        }

        private void Activate() {
            moveStart = Time.time;
            signal.Deactivate();
        }

        public void OnDeath(Mover m) {
            enemies.Remove(m);
            if (enemies.Count == 0) {
                Activate();
            }
        }
    }
}