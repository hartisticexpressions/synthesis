using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Camera
{
	public class Camera : MonoBehaviour
	{
        [SerializeField]
        Transform target;

        [SerializeField, Min(0f)]
        float distance = 5;

        [SerializeField, Range(0.01f, 0.5f)]
        float zoomSpeed = 0.1f;

        //[SerializeField, Range(1, 5)]
        //float zoomDamp = 2;

        [SerializeField, Range(1, 5)]
        float rotationDamp = 2;

        [SerializeField, Range(1,20)]
        float rotationSpeed = 4;

        [SerializeField, Range(0, 180)]
        float _yawLimit = 180;

        [SerializeField, Range(0, 90)]
        float _pitchLimit = 90;

        [SerializeField]
        bool freeze = false;

        public bool Freeze { get => freeze; set => freeze = value; }

        private Quaternion _pitch; //up and down
        private Quaternion _yaw; //left and right 

        private Quaternion _targetRotation;
        private Vector3 _targetPosition;
        //private float _targetDistance;

        void Awake()
        {
            _pitch = Quaternion.Euler(this.transform.rotation.eulerAngles.x, 0, 0);
            _yaw = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
        }

        private void Update()
        {
            if (!Freeze)
            {
                if (Input.GetMouseButton(0))
                    Move(Input.GetAxis("Mouse X") * rotationSpeed, -Input.GetAxis("Mouse Y") * rotationSpeed);
                distance = Mathf.Max(distance + Input.mouseScrollDelta.y * zoomSpeed, 0);
            }
        }

        void LateUpdate()
        {
            _targetRotation = _yaw * _pitch; //target angle from (0,0,0)

            //interpolate between current rotation to target rotation for smoothness
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _targetRotation, Mathf.Clamp01(Time.smoothDeltaTime * rotationDamp));

            Vector3 offset = this.transform.rotation * (-Vector3.forward * distance); //magnitude of offset (offset - origin) is equal to distance 
            this.transform.position = target.position + offset;
        }

        public void Move(float yawDelta, float pitchDelta)
        {
            _yaw = _yaw * Quaternion.Euler(0, yawDelta, 0);
            _pitch = _pitch * Quaternion.Euler(pitchDelta, 0, 0);
            ApplyConstraints();
        }

        private void ApplyConstraints()
        {
            Quaternion targetYaw = Quaternion.Euler(0, target.rotation.eulerAngles.y, 0);
            Quaternion targetPitch = Quaternion.Euler(target.rotation.eulerAngles.x, 0, 0);

            float yawDifference = Quaternion.Angle(_yaw, targetYaw);
            float pitchDifference = Quaternion.Angle(_pitch, targetPitch);

            float yawOverflow = yawDifference - _yawLimit;
            float pitchOverflow = pitchDifference - _pitchLimit;

            if (yawOverflow > 0) { _yaw = Quaternion.Slerp(_yaw, targetYaw, yawOverflow / yawDifference); }
            if (pitchOverflow > 0) { _pitch = Quaternion.Slerp(_pitch, targetPitch, pitchOverflow / pitchDifference); }
        }

    }
}
