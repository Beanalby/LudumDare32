using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public class SpawnPoint : MonoBehaviour {

        public void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) {
                return;
            }
            Debug.Log(Time.time + " " + name + " hit " + other.name + ", setting myself to active spawn point");
            GameDriver.Instance.spawnName = this.name;
        }

    }
}