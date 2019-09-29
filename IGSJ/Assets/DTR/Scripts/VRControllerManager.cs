using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRControllerManager : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean smackAction;

    #region Grabbing Objects
    private GameObject collidingObject; // 1
    private GameObject objectInHand; // 2
    #endregion

    #region Laser Pointer
    public GameObject laserPrefab; // 1
    private GameObject laser; // 2
    private Transform laserTransform; // 3
    private Vector3 hitPoint; // 4

    // 1
    public Transform cameraRigTransform;
    // 2
    public GameObject teleportReticlePrefab;
    // 3
    private GameObject reticle;
    // 4
    private Transform teleportReticleTransform;
    // 5
    public Transform headTransform;
    // 6
    public Vector3 teleportReticleOffset;
    // 7
    public LayerMask teleportMask;
    // 8
    private bool shouldTeleport;
    #endregion

    #region Grappling
    Vector3 start_pos = Vector3.zero;
    Vector3 final_pos = Vector3.zero;

    float fractionOfReset = 0;
    float speedOfGrapple = 0.02f;

    private bool isGrapplingOver = false;
    #endregion

    #region Launch arm
    Vector3 Arm_start_pos = Vector3.zero;
    Vector3 Arm_final_pos = Vector3.zero;

    float Arm_fractionOfReset = 0;
    float Arm_speedOfGrapple = 0.02f;


    public GameObject arm;
    private bool isLaunchingArm;
    public float launchSpeed = 40f;

    private Transform ParentOfArm;
    #endregion

    #region Smacking
    private Rigidbody rb;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // 1
        laser = Instantiate(laserPrefab);
        // 2
        laserTransform = laser.transform;
        // 1
        reticle = Instantiate(teleportReticlePrefab);
        // 2
        teleportReticleTransform = reticle.transform;

        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (smackAction.GetStateDown(handType))
        {
            this.GetComponent<Collider>().isTrigger = false;
            rb.isKinematic = false;
        }
        if (smackAction.GetStateUp(handType))
        {
            this.GetComponent<Collider>().isTrigger = true;
            rb.isKinematic = true;
        }

        if (teleportAction.GetState(handType) && shouldTeleport && grabAction.GetStateDown(handType) && !isLaunchingArm && arm.transform.parent != null)
        {
            launchArm();
        }

        if (isLaunchingArm)
        {
            if (arm.transform.position != Arm_final_pos)
            {
                arm.transform.parent = null;
                Arm_fractionOfReset += Arm_speedOfGrapple;
                if (Arm_fractionOfReset >= 1) Arm_fractionOfReset = 1;
                arm.transform.position = Vector3.Lerp(Arm_start_pos, Arm_final_pos, Arm_fractionOfReset);
            }
            else
            {
                isLaunchingArm = false;
                arm.transform.position = Arm_final_pos;
                Teleport();
            }
        }

        #region Grabbing
        // 1
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        // 2
        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }

        #endregion

        #region Movement
        if (teleportAction.GetState(handType))
        {
            RaycastHit hit;

            // 2
            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100, teleportMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
                // 1
                reticle.SetActive(true);
                // 2
                teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                // 3
                shouldTeleport = true;
            }
            else // 3
            {
                laser.SetActive(false);
                reticle.SetActive(false);
            }
        }
        else // 3
        {
            laser.SetActive(false);
            reticle.SetActive(false);
        }

        if (teleportAction.GetStateUp(handType) && shouldTeleport && !isGrapplingOver)
        {
            //Teleport();
        }

        if(isGrapplingOver)
        {
            if(cameraRigTransform.position != final_pos)
            {
                fractionOfReset += speedOfGrapple;
                if (fractionOfReset >= 1) fractionOfReset = 1;
                cameraRigTransform.position = Vector3.Lerp(start_pos, final_pos, fractionOfReset);
            }
            else
            {
                isGrapplingOver = false;
                cameraRigTransform.position = final_pos;
                shouldTeleport = false;
                arm.transform.parent = ParentOfArm;
                arm.transform.localPosition = Vector3.zero;
                arm.transform.localEulerAngles = new Vector3(90, 0, 0);
            }
        }
        #endregion
    }

    #region Launching arm
    public void launchArm()
    {
        ParentOfArm = arm.transform.parent;
        //arm.transform.position = arm.transform.position + arm.transform.up * 2;
        //Rigidbody rb = arm.GetComponent<Rigidbody>();
        //rb.velocity = arm.transform.up * launchSpeed;

        reticle.SetActive(false);
        
        Arm_start_pos = arm.transform.position;
        Arm_final_pos = hitPoint;

        Arm_fractionOfReset = 0;
        Arm_speedOfGrapple = 0.02f;
        float distance = Vector3.Distance(Arm_final_pos, Arm_start_pos);

        if (distance < 10)
        {
            speedOfGrapple = 0.04f;
        }

        isLaunchingArm = true;

        Vector3 difference = cameraRigTransform.position - headTransform.position;
        // 4
        difference.y = 0;
        // 5
        start_pos = cameraRigTransform.position;
        final_pos = hitPoint + difference;
        //cameraRigTransform.position = hitPoint + difference;

        fractionOfReset = 0;
        speedOfGrapple = 0.02f;
        distance = Vector3.Distance(final_pos, start_pos);

        if (distance < 10)
        {
            speedOfGrapple = 0.04f;
        }
    }
    #endregion

    #region Movement
    private void ShowLaser(RaycastHit hit)
    {
        // 1
        laser.SetActive(true);
        // 2
        laserTransform.position = Vector3.Lerp(controllerPose.transform.position, hitPoint, .5f);
        // 3
        laserTransform.LookAt(hitPoint);
        // 4
        laserTransform.localScale = new Vector3(laserTransform.localScale.x,
                                                laserTransform.localScale.y,
                                                hit.distance);
    }
    private void Teleport()
    {
        reticle.SetActive(false);
        

        //Vector3 difference = cameraRigTransform.position - headTransform.position;
        //// 4
        //difference.y = 0;
        //// 5
        //start_pos = cameraRigTransform.position;
        //final_pos = hitPoint + difference;
        ////cameraRigTransform.position = hitPoint + difference;

        //fractionOfReset = 0;
        //speedOfGrapple = 0.02f;
        //float distance = Vector3.Distance(final_pos, start_pos);

        //if(distance < 10)
        //{
        //    speedOfGrapple = 0.04f;
        //}

        isGrapplingOver = true;
    }

    #endregion


    #region grabbing objects

    private void SetCollidingObject(Collider col)
    {
        // 1
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        // 2
        if (col.tag == "Interactable" || col.tag == "Battery")
        {
            print(col.name);
            collidingObject = col.gameObject;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    // 3
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        // 1
        objectInHand = collidingObject;
        collidingObject = null;
        // 2
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    // 3
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        // 1
        if (GetComponent<FixedJoint>())
        {
            // 2
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            // 3
            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();

        }
        // 4
        objectInHand = null;
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        print("Magnitude of swing: " + Vector3.Magnitude(controllerPose.GetVelocity()));
        if(Vector3.Magnitude(controllerPose.GetVelocity()) > 5)
        {
            print("velocity of obj before swing: " + collision.rigidbody.velocity);
            collision.rigidbody.velocity *= 5;
            print("velocity of obj after swing: " + collision.rigidbody.velocity);
        }
    }
}
