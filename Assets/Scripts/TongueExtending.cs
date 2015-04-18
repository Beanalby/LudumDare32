using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public class TongueExtending : MonoBehaviour {
        [HideInInspector]
        public Transform mouth;
        public Tongue tongue;

        private float maxExpand = 5f;
        private float duration = .5f;

        private Vector3 launchPos, targetPos;
        private Vector3 gizmoPos = Vector3.zero;

        float expandStart = -1f;

        public void Start() {
            expandStart = Time.time;
            transform.position = mouth.position;
            transform.localScale = Vector3.zero;
            launchPos = transform.position;
        }

        public void FixedUpdate() {
            transform.position = mouth.position;
            float percent = (Time.time - expandStart) / duration;
            if (percent >= 1) {
                // we haven't hit anything, give up
                Destroy(gameObject);
                gizmoPos = Vector3.zero;
            } else {
                targetPos = launchPos + (percent * maxExpand) * Vector3.right;
                //gizmoPos = targetPos;
                Vector3 offset = targetPos - transform.position;
                float angle = Mathf.Rad2Deg * Mathf.Atan(offset.y / offset.x);
                //Debug.Log("mouth=" + transform.position + ", target=" + targetPos + ", offset=" + offset + ", angle=" + angle);
                // rotate us so the side ends up 
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.localScale = new Vector3(offset.magnitude, 1, 0);
            }
        }
        public void OnDrawGizmos() {
            if (gizmoPos != Vector3.zero) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(gizmoPos, .2f);
            }
        }

        public void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("Collided with " + other.name);
            tongue.Attach(other.gameObject, targetPos);
            Destroy(gameObject);
        }
    }
}