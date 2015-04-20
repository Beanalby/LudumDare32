using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    /// <summary>
    /// For simple objects that just get deleted when they die (boxes)
    /// </summary>
    public class DeleteOnDeath : MonoBehaviour {
        public void Die() {
            Destroy(gameObject);
        }
    }
}