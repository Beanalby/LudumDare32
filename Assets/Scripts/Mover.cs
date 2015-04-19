using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Mover : MonoBehaviour {
        public Sprite[] walkSprites;
        public Sprite jumpSprite;
        public float groundDampening = 20f;
        public float maxSpeed = 3;
        public float jumpSpeed = 10;

        private bool isJumping = false;
        private SpriteRenderer sr;
        private float walkCycleDelay = .7f;
        private int walkCycleIndex = 0;

        private CharacterController2D cc;

        void Start() {
            cc = GetComponent<CharacterController2D>();
            sr = GetComponentInChildren<SpriteRenderer>();
            StartCoroutine(WalkCycle());
            walkCycleDelay -= maxSpeed * .1f;
        }

        public void Jump() {
            if (cc.isGrounded) {
                isJumping = true;
            }
        }

        public void Move(float x) {
            if (!cc.isGrounded) {
                cc.velocity.y += Time.deltaTime * Physics2D.gravity.y;
            }
            if (isJumping) {
                cc.velocity.y += jumpSpeed;
                sr.sprite = jumpSprite;
                isJumping = false;
            }
            Vector3 newV = cc.velocity;
            newV.x = Mathf.Lerp(newV.x, x * maxSpeed, Time.fixedDeltaTime * groundDampening);
            cc.velocity = newV;


            // apply the new velocity to our position
            Vector3 delta = cc.velocity * Time.deltaTime;
            // flip ourselves if our direction changed
            if ((delta.x > 0 && transform.localScale.x < 0f)
                    || (delta.x < 0 && transform.localScale.x > 0f)) {
                transform.localScale = new Vector3(
                    -transform.localScale.x,
                    transform.localScale.y,
                    transform.localScale.z);
            }
            //cap delta based on our movement restrictions
            //if (delta.y > 0) {
            //    delta.y = Mathf.Min(Stage.yMax - transform.position.y, delta.y);
            //} else {
            //    delta.y = Mathf.Max(Stage.yMin - transform.position.y, delta.y);
            //}
            cc.move(delta);
        }

        public void FixedUpdate() {
            // check if we need to clear the jumpSprite
            if (cc.enabled && sr.sprite == jumpSprite && cc.isGrounded) {
                sr.sprite = walkSprites[walkCycleIndex];
            }
        }

        IEnumerator WalkCycle() {
            while (true) {
                if (cc.enabled && sr.sprite != jumpSprite) {
                    if (cc.velocity.magnitude > .1f) {
                        walkCycleIndex = (walkCycleIndex + 1) % walkSprites.Length;
                        sr.sprite = walkSprites[walkCycleIndex];
                    }
                }
                yield return new WaitForSeconds(walkCycleDelay);
            }
        }
    }
}