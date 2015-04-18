using UnityEngine;
using System.Collections;

public class TongueLink : MonoBehaviour {
    public int segmentNum;

    private HingeJoint2D joint;
    private float snapThreshold = .05f;
    Rigidbody2D body;

    public void Start() {
        joint = GetComponent<HingeJoint2D>();
        body = GetComponent<Rigidbody2D>();
    }
    public void FixedUpdate() {
        //SnapJoint();
    }

    public void SnapJoint() {
        // we don't want spring behavior, the tongue segments separate and
        // it causes gaps in the tongue and it's creepy as hell.
        // We want to snap immediately if what we're connected
        // to moves.  Figure out the the world coordinates of our anchor and connectedAnchor,
        // and if there's any distance, move us closer.
        Vector3 anchor = transform.position + (transform.rotation * joint.anchor);
        Rigidbody2D connected = joint.connectedBody;
        Vector3 tmpAnchor = Vector3.Scale(Quaternion.Euler(0, 0, connected.rotation) * (Vector3)joint.connectedAnchor, connected.transform.localScale);
        Vector3 connectedAnchor = (Vector3)connected.position + tmpAnchor;

        Vector3 offset = (connectedAnchor - anchor);
        if (offset.magnitude > snapThreshold) {
            // don't move ALL the way, leave a little bit for the
            // physics engine, which helps give proper rotation
            offset *= .8f;
            body.position = body.position + (Vector2)offset;
        }
    }
}
