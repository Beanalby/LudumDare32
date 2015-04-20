using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public delegate void DeathListener(Mover obj);

    [RequireComponent(typeof(CharacterController2D))]
    public class Mover : MonoBehaviour {

        public Sprite[] walkSprites;
        public Sprite jumpSprite;
        public Sprite deathSprite;
        public float groundDampening = 20f;
        public float maxSpeed = 3;
        public float jumpSpeed = 10;
        public DeathListener OnDeath;

        private bool isJumping = false;
        private SpriteRenderer sr;
        private Rigidbody2D rb;
        private float walkCycleDelay = .7f;
        private int walkCycleIndex = 0;
        
        private CharacterController2D cc;

        void Start() {
            cc = GetComponent<CharacterController2D>();
            sr = GetComponentInChildren<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
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
        public void Die() {
            cc.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            rb.isKinematic = false;
            rb.velocity = new Vector2(Random.Range(-5, 5), 8);
            float angle = Random.Range(60, 120);
            if (Random.Range(0, 2) == 0) {
                angle = -angle;
            }
            rb.angularVelocity = angle;
            sr.sprite = deathSprite;
            Destroy(gameObject, 5f);
            this.enabled = false;
            if (OnDeath != null) {
                OnDeath(this);
            }
       }
    }
}