using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public class GameDriver : MonoBehaviour {
        public static GameDriver Instance {
            get { return _instance; }
        }
        private static GameDriver _instance = null;

        public void Awake() {
            if (_instance != null) {
                Debug.LogError("Can't have two gameDrivers!");
                Destroy(gameObject);
            }
            _instance = this;
        }

        public void PlayerDied() {
            StartCoroutine(_playerDied());
        }

        private IEnumerator _playerDied() {
            yield return new WaitForSeconds(2);
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}