using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public class TongueFactory : MonoBehaviour {
        public GameObject tonguePrefab;
        private int numSegments = 20;

        private Vector2 connectedAnchor = new Vector2(0.53f, 0.42f);
        //private Vector3 positionOffset = new Vector3(0.164f, 0f, 0);
        private Vector3 positionOffset = new Vector3(0.39f, 0f, 0);

        public void Start() {
            // first link connects to us
            Rigidbody2D previousLink = GetComponent<Rigidbody2D>();

            for (int i = 0; i < numSegments; i++) {
                Vector3 pos = i * positionOffset;
                GameObject obj = Instantiate(tonguePrefab, pos, Quaternion.identity) as GameObject;
                HingeJoint2D joint = obj.GetComponent<HingeJoint2D>();
                joint.connectedBody = previousLink;
                if (i == 0) {
                    // first link attaches to us at a different position
                    joint.connectedAnchor = connectedAnchor;
                }
                previousLink = joint.GetComponent<Rigidbody2D>();
                joint.GetComponent<TongueLink>().segmentNum = i;
            }
        }
    }
}