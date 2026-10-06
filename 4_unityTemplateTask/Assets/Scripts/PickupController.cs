using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PickupController : MonoBehaviour
{
    [Header( "Pickup settings" )]
    [SerializeField] Transform holdArea;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform playerTransform;

    private GameObject heldObject;
    private Rigidbody heldObjectRB;
    private XRGrabInteractable heldObjectGI;

    [Header( "Physics Parameters" )]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private void Update() {
        Debug.DrawRay( transform.position, transform.forward * pickupRange, Color.red );
        if (Input.GetMouseButtonDown(0)) {
            if( heldObject == null ) {
                RaycastHit hit;                
                //if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hit, pickupRange ) ) {
                if( Physics.Raycast( cameraTransform.position, cameraTransform.forward, out hit, pickupRange ) ) {
                    if( hit.rigidbody != null ) {
                        PickupObject( hit.rigidbody.gameObject, true );
                    }
                }
            } else {
                PickupObject( null, false );
            }
        }

        if( heldObject != null ) {
            MoveObject();
        }
    }

    void PickupObject(GameObject pickedObject, bool isPickup ) {
        if ( isPickup ) {
            if( pickedObject.GetComponent<Rigidbody>() ) {
                heldObjectGI = pickedObject.GetComponent<XRGrabInteractable>();
                heldObjectGI.enabled = false;
                heldObjectRB = pickedObject.GetComponent<Rigidbody>();
                heldObjectRB.useGravity = false;
                heldObjectRB.linearDamping = 10;
                heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;
                heldObjectRB.transform.parent = playerTransform;
                heldObject = pickedObject;
            }
        } else {
            heldObjectGI.enabled = true;
            heldObjectGI = null;
            heldObjectRB.useGravity = true;
            heldObjectRB.linearDamping = 1;
            heldObjectRB.constraints = RigidbodyConstraints.None;
            heldObjectRB.transform.parent = null;
            heldObject = null;
        }
    }

    void MoveObject() {
        if( Vector3.Distance( heldObject.transform.position, holdArea.position ) > 0.1f ) {
            Vector3 moveDirection = holdArea.position - heldObject.transform.position;
            heldObjectRB.AddForce( moveDirection * pickupForce );
        }
    }
}
