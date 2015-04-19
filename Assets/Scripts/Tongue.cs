using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public class Tongue : MonoBehaviour {
        public TongueLink tongueLinkPrefab;
        public TongueExtending extendPrefab;
        public Transform mouth;
        public Sprite tongueSprite;

        private int minSegments = 10;
        private HingeJoint2D target;
        private float tongueLength; // derived automatically at start
        private TongueLink[] links = null;

        public void Start() {
            HingeJoint2D joint = tongueLinkPrefab.GetComponent<HingeJoint2D>();
            tongueLength = (joint.anchor - joint.connectedAnchor).magnitude;
        }

        public void Update() {
            if (Input.GetButtonDown("Fire1")) {
                if (links == null) {
                    Extend();
                    //Attach(null, transform.position + 5 * Vector3.right);
                    //Attach(null);
                } else {
                    Detach();
                }
            }
        }

        private void CreateSegments(Vector3 targetPos) {
            // first link connects to us
            Rigidbody2D previousLink = GetComponent<Rigidbody2D>();
            //int numSegments = Mathf.CeilToInt((targetPos - mouth.position).magnitude / tongueLength);
            int numSegments = Mathf.RoundToInt((targetPos - mouth.position).magnitude / tongueLength);
            // always make at least a few segments, otherwise things get weird
            numSegments = Mathf.Max(numSegments, minSegments);
            // figure out the angle between us and the target, and set positionOffset accordingly
            Vector3 offset = targetPos - mouth.position;
            float angle = Mathf.Rad2Deg * Mathf.Atan(offset.y / offset.x);
            if (targetPos.x - mouth.position.x < 0) {
                angle += 180;
            }
            Vector3 positionOffset = new Vector3(tongueLength, 0f, 0);
            Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));
            positionOffset = rot * positionOffset;
            links = new TongueLink[numSegments];
            for (int i = 0; i < numSegments; i++) {
                Vector3 pos = (Vector3)previousLink.position;
                if(i!=0) {
                    pos += positionOffset;
                }
                GameObject obj = Instantiate(tongueLinkPrefab.gameObject, pos, rot) as GameObject;
                HingeJoint2D joint = obj.GetComponent<HingeJoint2D>();
                joint.connectedBody = previousLink;
                if (i == 0) {
                    // first link attaches to us at a different position.
                    // Set the anchor, and move the position to it
                    joint.connectedAnchor = mouth.transform.localPosition;
                    // move the segment so the anchor and connectedAnchor overlap
                    Vector3 tmpAnchor = Vector3.Scale(Quaternion.Euler(0, 0, previousLink.rotation) * (Vector3)joint.connectedAnchor, previousLink.transform.localScale) - (Vector3)joint.anchor;
                    joint.transform.position = (Vector3)previousLink.position + tmpAnchor;
                }
                previousLink = joint.GetComponent<Rigidbody2D>();
                links[i] = joint.GetComponent<TongueLink>();
                links[i].segmentNum = i;
            }
        }

        public void Attach(GameObject newTarget, Vector3 attachPos) {
            // TODO attach to the target!
            CreateSegments(attachPos);
            if (newTarget != null) {
                target = newTarget.GetComponent<HingeJoint2D>();
                target.connectedBody = links[links.Length - 1].GetComponent<Rigidbody2D>();
                target.anchor = target.transform.InverseTransformPoint(attachPos);
                target.enabled = true;
                target.SendMessage("TongueAttached", attachPos, SendMessageOptions.DontRequireReceiver);
            }
        }

        public void Extend() {
            TongueExtending extend = Instantiate(extendPrefab.gameObject).GetComponent<TongueExtending>();
            if (transform.localScale.x < 0) {
                extend.direction = -1;
            }
            extend.mouth = mouth;
            extend.tongue = this;
        }

        public void Detach() {
            foreach (TongueLink link in links) {
                Destroy(link.gameObject);
            }
            links = null;
            if (target != null) {
                target.SendMessage("TongueDetached", SendMessageOptions.DontRequireReceiver);
                target.enabled = false;
                target = null;
            }
        }
    }
}