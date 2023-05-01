
using BendyArms;
using Cinemachine;
using RootMotion.FinalIK;

using UnityEngine;
using UnityEngine.Serialization;

public class TentacleController : MonoBehaviour
{
    [SerializeField] private PlayerInputControllerRewired playerInput;
    [SerializeField] private Transform hoverTransform;
    [SerializeField] private Rigidbody boatRigidbody;   //used to set velocity of crates when releasing them 
    [SerializeField] private CinemachineVirtualCamera cmVirtualCamera;

    [SerializeField] private Camera mainCamera;

    [FormerlySerializedAs("tentacleTarget")] [SerializeField] private Transform cursorMain;
    [SerializeField] private Transform tentacleLeftOrigin;
    [SerializeField] private Transform tentacleRightOrigin;
    [SerializeField] private Transform tentacleRightFinalLimb;
    [SerializeField] private Transform tentacleLeftFinalLimb;
    [SerializeField] private Transform cursorRightDefault;
    [SerializeField] private Transform cursorLeftDefault;
    [SerializeField] private TentacleCursorDamped cursorRight;
    [SerializeField] private TentacleCursorDamped cursorLeft;
    [SerializeField] private TentacleCollider tentacleColliderRight;
    [SerializeField] private TentacleCollider tentacleColliderLeft;
    
    
    
    [SerializeField] private CCDIK ikLeft;
    [SerializeField] private CCDIK ikRight;
    [SerializeField] private float distMult = 2f;
    [SerializeField] private float minWeight = 0.1f;
    [SerializeField] private float maxReleaseVelocity = 30f;
    
    [SerializeField] private LayerMask layersToPickUp;
    [SerializeField] private LayerMask layersToMouseOver;
    
    private TentacleSide currentSide = TentacleSide.Left;

    private Rigidbody carriedObjectRb;
    private Rigidbody targetObject;
    public enum TentacleSide
    {
        Right,
        Left
    }

    [SerializeField] public FMOD.Studio.EventInstance pickUpSound;
    [SerializeField] public FMOD.Studio.EventInstance dropSound;

    public static int containerCount = 0;


    private enum TentacleState
    {
        Free,
        MovingToTarget,
        HoldingObject
    }

    private TentacleState state;
    
    private void OnEnable()
    {
        playerInput.firePrimary.down += TryToPickUp;
        playerInput.firePrimary.up += TryToRelease;

        tentacleColliderLeft.tentacleCollided += TentacleCollidedLeft;
        tentacleColliderRight.tentacleCollided += TentacleCollidedRight;
    }

    private void OnDisable()
    {
        playerInput.firePrimary.down -= TryToPickUp;
        playerInput.firePrimary.up -= TryToRelease;
        
        tentacleColliderLeft.tentacleCollided -= TentacleCollidedLeft;
        tentacleColliderRight.tentacleCollided -= TentacleCollidedRight;
    }

    private void TryToPickUp()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.Log("Trying to pick up");
        if (Physics.Raycast(ray, out hit, 9999f, layersToPickUp))
        {
            
            if (hit.rigidbody)
            {
                targetObject = hit.rigidbody;
                SetState(TentacleState.MovingToTarget);
                pickUpSound = FMODUnity.RuntimeManager.CreateInstance("event:/Sfx/Gameplay/Pickup");
                containerCount += 1;
                // Colin I'm taking the jump to behavior
                if (currentSide == TentacleSide.Left)
                {
                    cursorLeft.SetCursorTransform(targetObject.transform);  //make the tentacle move towards the target object 
                    pickUpSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(tentacleLeftFinalLimb));
                }
                else
                {
                    cursorRight.SetCursorTransform(targetObject.transform);
                    pickUpSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(tentacleRightFinalLimb));
                }
                
                pickUpSound.start();
                pickUpSound.release();
                
            }
        }
    }

    public void TentacleCollidedRight(Collider hitCollider)
    {
        PickupObject(hitCollider);
    }
    public void TentacleCollidedLeft(Collider hitCollider)
    {
        PickupObject(hitCollider);
    }

    void PickupObject(Collider hitCollider)
    {
        if (state != TentacleState.MovingToTarget)
            return;
        
        var rb = hitCollider.GetComponent<Rigidbody>();
        if (!rb)
            return;

        Debug.Log("TentacleController: Got valid collision pick up event");
        carriedObjectRb = rb;
        rb.interpolation = RigidbodyInterpolation.None;
        rb.isKinematic = true;
        if (currentSide == TentacleSide.Left)
        {
            rb.transform.SetParent(tentacleColliderLeft.transform);
            cursorLeft.SetCursorTransform(cursorMain); 
        }
        if (currentSide == TentacleSide.Right)
        {
            rb.transform.SetParent(tentacleColliderRight.transform);
            cursorRight.SetCursorTransform(cursorMain); 
        }
        SetState(TentacleState.HoldingObject);
    }
    private void SetState(TentacleState newState)
    {
        state = newState;
        Debug.Log("Tentacle: Setting state to: "+state.ToString());
        //do other stuff on state transition if needed
        if (state == TentacleState.Free)
        {
            if (!carriedObjectRb)
                return;
            carriedObjectRb.isKinematic = false;
            carriedObjectRb.interpolation = RigidbodyInterpolation.Interpolate;
            carriedObjectRb.transform.parent = null;

            //release the crate w the velocity of the boat plus the velocity of the current cursor ...  
            //i think the cursor velocity tends to include the boat velocity, so only adding the cursor velocity right now
            
            var objectVelocity = Vector3.zero;//boatRigidbody.velocity;
            if (currentSide == TentacleSide.Left)
                objectVelocity += cursorLeft.Velocity;
            if (currentSide == TentacleSide.Right)
                objectVelocity += cursorRight.Velocity;
            
            
            carriedObjectRb.velocity = Vector3.ClampMagnitude(objectVelocity, maxReleaseVelocity);
            carriedObjectRb = null;
            
            
            //reset cursor from target object to default cursor 
            if (currentSide == TentacleSide.Left)
                cursorLeft.SetCursorTransform(cursorMain);
            else
                cursorRight.SetCursorTransform(cursorMain);
        }
        else if (state == TentacleState.HoldingObject)
        {
            
        }
    }

    private void TryToRelease()
    {
        if (state == TentacleState.Free)
            return;

        if (state == TentacleState.MovingToTarget) // were trying to pick something up but released button before it collided 
        {
            SetState(TentacleState.Free);
        }
        else if (state == TentacleState.HoldingObject)
        {
            dropSound = FMODUnity.RuntimeManager.CreateInstance("event:/Sfx/Gameplay/Drop");
            if (currentSide == TentacleSide.Left)
            {
                Debug.Log("Setting left");
                dropSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(tentacleLeftFinalLimb));
            }
            else
            {
                dropSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(tentacleRightFinalLimb));
            }

            dropSound.start();
            dropSound.release();
            SetState(TentacleState.Free);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        RunState();
    }

    void RunState()
    {
        if (state == TentacleState.Free)
        {
            UpdateCursors();
        }
        else if (state == TentacleState.MovingToTarget)
        {
            
        }
        else if (state == TentacleState.HoldingObject)
        {
            UpdateCursors();
        }
    }
    void UpdateCursors()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, 9999f, layersToMouseOver)) {
            Transform objectHit = hit.transform;
            
            // Do something with the object that was hit by the raycast.
            cursorMain.position = new Vector3(hit.point.x, hoverTransform.position.y, hit.point.z);

            var distRight = Vector3.Distance(cursorMain.position, tentacleRightOrigin.position);
            var distLeft = Vector3.Distance(cursorMain.position, tentacleLeftOrigin.position);
            var weightLeft = Mathf.Clamp((distRight - distLeft) / distMult, minWeight, 1f); 
            var weightRight = Mathf.Clamp((distLeft - distRight) / distMult, minWeight, 1f); 
            //Debug.Log("right: "+distRight.ToString()+"weight: "+weightRight.ToString()+" left: "+distLeft.ToString()+" weight: "+weightLeft.ToString() );

            if (state == TentacleState.HoldingObject)   //don't switch hands if we are holding an object 
            {
                return;
            }

            if (distRight < distLeft)
            {
                currentSide = TentacleSide.Right;
                cursorRight.SetCursorTransform(cursorMain); 
                cursorLeft.SetCursorTransform(cursorLeftDefault);
            }
            else
            {
                currentSide = TentacleSide.Left;
                cursorRight.SetCursorTransform(cursorRightDefault); 
                cursorLeft.SetCursorTransform(cursorMain);
            }
     
            ikLeft.solver.IKPositionWeight = weightLeft;
            ikRight.solver.IKPositionWeight = weightRight;

            
        }
    }
}
