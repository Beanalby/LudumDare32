using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public class Tongue : MonoBehaviour {
        public GameObject tonguePrefab;

        private GameObject target;
        private float tongueLength; // derived automatically at start
        private Vector2 connectedAnchor = new Vector2(0.53f, 0.42f);
        //private Vector3 positionOffset = new Vector3(0.164f, 0f, 0);
        private TongueLink[] links = null;

        public void Start() {
            HingeJoint2D joint = tonguePrefab.GetComponent<HingeJoint2D>();
            tongueLength = (joint.anchor - joint.connectedAnchor).magnitude;
        }

        public void Update() {
            if (Input.GetButtonDown("Fire1")) {
                if (links == null) {
                    Attach(null);
                } else {
                    Remove();
                }
            }
        }

        private void Create(int numSegments) {
            // first link connects to us
            Vector3 positionOffset = new Vector3(tongueLength, 0f, 0);
            Rigidbody2D previousLink = GetComponent<Rigidbody2D>();
            links = new TongueLink[numSegments];
            for (int i = 0; i < numSegments; i++) {
                Vector3 pos = (Vector3)previousLink.position;
                if(i!=0) {
                    pos += positionOffset;
                }
                GameObject obj = Instantiate(tonguePrefab, pos, Quaternion.identity) as GameObject;
                HingeJoint2D joint = obj.GetComponent<HingeJoint2D>();
                joint.connectedBody = previousLink;
                if (i == 0) {
                    // first link attaches to us at a different position.
                    // Set the anchor, and move the position to it
                    joint.connectedAnchor = connectedAnchor;
                    // move the segment so the anchor and connectedAnchor overlap
                    Vector3 tmpAnchor = Vector3.Scale(Quaternion.Euler(0, 0, previousLink.rotation) * (Vector3)joint.connectedAnchor, previousLink.transform.localScale) - (Vector3)joint.anchor;
                    joint.transform.position = (Vector3)previousLink.position + tmpAnchor;
                }
                previousLink = joint.GetComponent<Rigidbody2D>();
                links[i] = joint.GetComponent<TongueLink>();
                links[i].segmentNum = i;
            }
        }

        public void Attach(GameObject newTarget) {
            // TODO attach to the target!
            Create(20);
        }

        public void Remove() {
            foreach (TongueLink link in links) {
                Destroy(link.gameObject);
            }
            links = null;
        }
    }
}