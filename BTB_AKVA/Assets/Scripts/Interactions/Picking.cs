using UnityEngine;
using AKVA.Player;

namespace AKVA.Interaction
{
    public class Picking : MonoBehaviour
    {
        [SerializeField] private LayerMask PickupMask;
        [SerializeField] private Camera PlayerCam;
        [SerializeField] private Transform PickupTarget;
        [Space]
        [SerializeField] private float PickupRange;
        private Rigidbody CurrentObject;

        void Update()
        {
            if(Input.GetKeyDown(PlayerInput.Instance.Controls.pick))
            {
                if(CurrentObject)
                {
                    CurrentObject.useGravity = true;
                    CurrentObject = null;
                    return;
                }

                Ray CameraRay = PlayerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                if(Physics.Raycast(CameraRay, out RaycastHit HitInfo, PickupRange, PickupMask))
                {
                    CurrentObject = HitInfo.rigidbody;
                    CurrentObject.useGravity = false;
                }
            }
        }

        void FixedUpdate()
        {
            if(CurrentObject)
            {
                Vector3 DirectionToPoint = PickupTarget.position - CurrentObject.position;
                float DistanceToPoint = DirectionToPoint.magnitude;

                CurrentObject.transform.forward = transform.forward;
                CurrentObject.velocity = DirectionToPoint * 12f * DistanceToPoint;
            }
        }
    }
}