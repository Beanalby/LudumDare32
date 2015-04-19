using UnityEngine;
using System.Collections;

namespace LudumDare32
{
    [RequireComponent(typeof(Mover))]
    public class Player : MonoBehaviour
    {
        private Mover mover;
        private CharacterController2D cc;
        private bool retryJump = false;

        public void Start() {
            mover = GetComponent<Mover>();
            cc = GetComponent<CharacterController2D>();
        }
        public void Update() {
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
   }
}