using UnityEngine;
using System.Collections;
using Moments;

namespace LudumDare32 {
    [RequireComponent(typeof(Recorder))]
    public class RecordController : MonoBehaviour {
        private Recorder recorder;
        public void Start() {
            recorder = GetComponent<Recorder>();
            recorder.Record();
        }
        public void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                recorder.Save();
            }
        }
    }
}