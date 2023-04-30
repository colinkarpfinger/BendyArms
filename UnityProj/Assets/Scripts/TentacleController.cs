
using BendyArms;
using Cinemachine;
using RootMotion.FinalIK;

using UnityEngine;
using UnityEngine.Serialization;

public class TentacleController : MonoBehaviour
{
    [SerializeField] private PlayerInputControllerRewired playerInput;
    [SerializeField] private Transform hoverTransform;
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
    
    
    
    [SerializeField] private CCDIK ikLeft;
    [SerializeField] private CCDIK ikRight;
    [SerializeField] private float distMult = 2f;
    [SerializeField] private float minWeight = 0.1f;
    
    [SerializeField] private LayerMask layersToPickUp;
    [SerializeField] private LayerMask layersToMouseOver;

    private bool isHoldingObject = false;
    private TentacleSide currentSide = TentacleSide.Left;

    private Rigidbody carriedObjectRb;
    public enum TentacleSide
    {
        Right,
        Left
    }
    private void OnEnable()
    {
        playerInput.firePrimary.down += TryToPickUp;
        playerInput.firePrimary.up += TryToRelease;
    }

    private void OnDisable()
    {
        playerInput.firePrimary.down -= TryToPickUp;
        playerInput.firePrimary.up -= TryToRelease;
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
                isHoldingObject = true;
                carriedObjectRb = hit.rigidbody;
                hit.rigidbody.isKinematic = true;
                
                if (currentSide == TentacleSide.Left)
                {
                    var offset = hit.point - hit.transform.position;
                    Debug.Log("offset; "+offset.ToString());
                    hit.transform.parent = tentacleLeftFinalLimb.parent;
                    hit.transform.localPosition = Vector3.zero;
                    hit.transform.localRotation = Quaternion.identity;
                    // hit.transform.rotation =
                  //      Quaternion.LookRotation(tentacleLeftFinalLimb.right, tentacleLeftFinalLimb.up);
                }
                else
                {
                    hit.transform.parent = tentacleRightFinalLimb.parent;
                }
                
                
                
            }
        }
    }

    public void TentacleCollided(Collider collider)
    {
        
    }

    private void TryToRelease()
    {
        if (!carriedObjectRb)
            return;
        carriedObjectRb.isKinematic = false;
        //carriedObjectRb
        carriedObjectRb.transform.parent = null;
        
        isHoldingObject = false;
        carriedObjectRb = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursors();
    }

    void UpdateCursors()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, 9999f, layersToMouseOver)) {
            Transform objectHit = hit.transform;
            
            // Do something with the object that was hit by the raycast.
            cursorMain.position = new Vector3(hit.point.x, hoverTransform.position.y, hit.point.z);


            if (isHoldingObject)
                return;
            
            var distRight = Vector3.Distance(cursorMain.position, tentacleRightOrigin.position);
            var distLeft = Vector3.Distance(cursorMain.position, tentacleLeftOrigin.position);
            var weightLeft = Mathf.Clamp((distRight - distLeft) / distMult, minWeight, 1f); 
            var weightRight = Mathf.Clamp((distLeft - distRight) / distMult, minWeight, 1f); 
            //Debug.Log("right: "+distRight.ToString()+"weight: "+weightRight.ToString()+" left: "+distLeft.ToString()+" weight: "+weightLeft.ToString() );


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
