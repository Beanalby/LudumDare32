using UnityEngine;
using System.Collections;

namespace LudumDare32 {
    [RequireComponent(typeof(SpringJoint2D),typeof(SliderJoint2D))]
    public class Scale : MonoBehaviour {

        public GameObject target;
        private SpringJoint2D spring;
        private SliderJoint2D slider;
        Rigidbody2D platform;
        private float baseDistance;

        public void Start() {
            spring = GetComponent<SpringJoint2D>();
            slider = GetComponent<SliderJoint2D>();
            platform = spring.connectedBody;

            // set default distance and limits based on what we're connected to
            baseDistance = transform.position.y - platform.transform.position.y;
            spring.distance = baseDistance - 1;
            slider.useLimits = true;
            slider.limits = new JointTranslationLimits2D() { min = -(baseDistance + 2), max = -(baseDistance - 1) };
        }

        public void Update() {
            float delta = (transform.position.y - platform.transform.position.y) - baseDistance;
            delta = Mathf.Max(0, delta);
            target.SendMessage("SetScaleDelta", delta);
        }
    }
}