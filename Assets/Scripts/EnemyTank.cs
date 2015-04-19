using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    [RequireComponent(typeof(Mover))]
    public class EnemyTank : MonoBehaviour {

        public Transform tex;

        private float moveDir = 1;
        private bool isTongued = false;
        private CharacterController2D cc;
        private Rigidbody2D rb;
        private Mover mover;
        private float upgrightThresold = 5;

        public void Start() {
            cc = GetComponent<CharacterController2D>();
            mover = GetComponent<Mover>();
            rb = GetComponent<Rigidbody2D>();
        }
        
        public void BorderEntered(Transform border) {
            // we hit an invisible border, turn around
            if (border.position.x > transform.position.x) {
                moveDir = -1;
            } else {
                moveDir = 1;
            }
        }

        public void FixedUpdate() {
            if (isTongued) {
                return;
            }
            if (!rb.isKinematic) {
                // we're not kinematic, check if we're upright enough to enable it
                if (isUpright()) {
                    rb.isKinematic = true;
                    // get rid of any small rotation left over
                    transform.rotation = Quaternion.identity;
                }
            }
            if (rb.isKinematic) {
                if (cc.isGrounded) {
                    // only move if we're straight up, not when rotated
                    mover.Move(moveDir);
                } else {
                    mover.Move(0);
                }
            }
        }

        public void TongueAttached(Vector3 attachPos) {
            isTongued = true;
            rb.isKinematic = false;
        }

        public void TongueDetached() {
            isTongued = false;
            // we might need to leave it nonkinematic if we're not upright,
            // let FixedUpdate handle it
        }

        private bool isUpright() {
            float angle = transform.rotation.eulerAngles.z;
            if (angle > 180) {
                angle = 360 - angle;
            }
            return angle < upgrightThresold;
        }
    }
}