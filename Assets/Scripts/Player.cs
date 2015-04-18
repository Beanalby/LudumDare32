using UnityEngine;
using System.Collections;

namespace LudumDare32
{
    [RequireComponent(typeof(Mover))]
    public class Player : MonoBehaviour
    {
        private Mover mover;

        public void Start() {
            mover = GetComponent<Mover>();
        }
        public void Update() {
            if (Input.GetButtonDown("Jump")) {
                mover.Jump();
            }
        }
        public void FixedUpdate() {
            mover.Move(Input.GetAxis("Horizontal"));
        }
   }
}