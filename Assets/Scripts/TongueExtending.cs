using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public class TongueExtending : MonoBehaviour {
        [HideInInspector]
        public Transform mouth;
        public Tongue tongue;
        public Transform extendingTexture;
        public Transform extendingCollider;

        [HideInInspector]
        public float direction = 1;

        private float maxExpand = 5f;
        private float duration = .5f;

        private Vector3 launchPos, targetPos;
        private Vector3 gizmoPos = Vector3.zero;

        float expandStart = -1f;

        public void Start() {
            expandStart = Time.time;
            transform.position = mouth.position;
            extendingTexture.localScale = Vector3.up;
            launchPos = transform.position;
            extendingCollider.transform.localPosition = Vector3.zero;
            if (direction == -1) {
                extendingCollider.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        public void FixedUpdate() {
            transform.position = mouth.position;
            float percent = (Time.time - expandStart) / duration;
            if (percent >= 1) {
                // we haven't hit anything, give up
                tongue.Attach(null, targetPos);
                Destroy(gameObject);
                gizmoPos = Vector3.zero;
            } else {
                targetPos = launchPos + (percent * maxExpand) * (direction * Vector3.right);

                gizmoPos = targetPos;
                Vector3 offset = targetPos - transform.position;
                float angle = Mathf.Rad2Deg * Mathf.Atan(offset.y / offset.x);
                //Debug.Log("mouth=" + transform.position + ", target=" + targetPos + ", offset=" + offset + ", angle=" + angle);
                // rotate us so the side ends up 
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                extendingTexture.localScale = new Vector3(direction * offset.magnitude, 1, 0);
                extendingCollider.transform.position = targetPos;
            }
        }
        public void OnDrawGizmos() {
            if (gizmoPos != Vector3.zero) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(gizmoPos, .2f);
            }
        }

        public void OnTriggerEnter2D(Collider2D other) {
            // triggers don't count!
            if (other.isTrigger) {
                return;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                return;
            }
            // we hit something, but only attach to it if it's attackable
            GameObject target = null;
            if (other.gameObject.layer == LayerMask.NameToLayer("Attackable")) {
                target = other.gameObject;
            }
            tongue.Attach(target, targetPos);
            gizmoPos = targetPos;
            Destroy(gameObject);
        }
    }
}