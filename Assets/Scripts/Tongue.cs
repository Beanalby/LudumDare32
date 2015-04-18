using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    public class Tongue : MonoBehaviour {
        public TongueLink tongueLinkPrefab;
        public TongueExtending extendPrefab;
        public Transform mouth;
        public Sprite tongueSprite;

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
                    Remove();
                }
            }
        }

        private void Create(Vector3 targetPos) {
            // first link connects to us
            Rigidbody2D previousLink = GetComponent<Rigidbody2D>();
            //int numSegments = Mathf.CeilToInt((targetPos - mouth.position).magnitude / tongueLength);
            int numSegments = Mathf.RoundToInt((targetPos - mouth.position).magnitude / tongueLength);
            // figure out the angle between us and the target, and set positionOffset accordingly
            Vector3 offset = targetPos - mouth.position;
            float angle = Mathf.Rad2Deg * Mathf.Atan(offset.y / offset.x);
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
            Create(attachPos);
            if (newTarget != null) {
                target = newTarget.GetComponent<HingeJoint2D>();
                target.connectedBody = links[links.Length - 1].GetComponent<Rigidbody2D>();
                Debug.Log("target at " + target.transform.position + ", attachPos=" + attachPos + ", anchorPos = " + target.anchor + ", inverseAttach gives + " + target.transform.InverseTransformPoint(attachPos));
                target.anchor = target.transform.InverseTransformPoint(attachPos);
                target.enabled = true;
            }
            //Debug.Break();
        }

        public void Extend() {
             TongueExtending extend = Instantiate(extendPrefab.gameObject).GetComponent<TongueExtending>();
            extend.mouth = mouth;
            extend.tongue = this;
        }

        public void Remove() {
            foreach (TongueLink link in links) {
                Destroy(link.gameObject);
            }
            links = null;
            if (target != null) {
                target.enabled = false;
                target = null;
            }
        }
    }
}