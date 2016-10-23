using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace LudumDare32 {

    public class GameDriver : MonoBehaviour {
        public static GameDriver Instance {
            get { return _instance; }
        }
        private static GameDriver _instance = null;

        public string spawnName;

        public void Awake() {
            if (_instance != null && Instance != this) {
                // there was already a GameDriver, get rid of this one
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(transform.gameObject);
            _instance = this;
        }
        public void OnEnable() {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
        public void Start() {
            InitLevel();
        }
        public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
            InitLevel();
        }

        public void InitLevel() {
            if (spawnName != "") {
                // move the player to the active spawn point
                GameObject spawn = GameObject.Find(spawnName);
                if (spawn == null) {
                    Debug.LogError("Can't find spawnpoint [" + spawnName + "]!");
                    Debug.Break();
                    return;
                }
                Player player = GameObject.FindObjectOfType<Player>();
                player.transform.position = spawn.transform.position;
                // move the camera to the player immedaitely
                Vector3 pos = Camera.main.transform.position;
                pos.x = player.transform.position.x;
                Camera.main.transform.position = pos;
            }
        }

        public void PlayerDied() {
            StartCoroutine(_playerDied());
        }

        private IEnumerator _playerDied() {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}