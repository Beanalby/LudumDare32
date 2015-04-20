using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    [RequireComponent(typeof(Mover))]
    public class Player : MonoBehaviour
    {
        private Mover mover;
        private CharacterController2D cc;
        private bool retryJump = false;
        private Tongue tongue;

        public void Start() {
            mover = GetComponent<Mover>();
            tongue = GetComponent<Tongue>();
            cc = GetComponent<CharacterController2D>();
        }
        public void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                mover.Die();
                Die();
            }
            if (Input.GetButtonDown("Fire1")) {
                tongue.ActivateTongue();
            }
            if (Input.GetButtonDown("Jump")) {
                if (cc.isGrounded) {
                    mover.Jump();
                } else {
                    // keep trying to jump next frame
                    retryJump = true;
                }
            } else if (retryJump) {
                if (Input.GetButton("Jump")) {
                    if(cc.isGrounded) {
                        mover.Jump();
                        retryJump = false;
                    }
                } else {
                    // they let go of jump before we got grounded, stop retrying
                    retryJump = false;
                }
            }
        }
        public void FixedUpdate() {
            mover.Move(Input.GetAxis("Horizontal"));
        }
        public void Die() {
            // Move takes care of the actual death
            this.enabled = false;
            GameDriver.Instance.SendMessage("PlayerDied");
        }
   }
}