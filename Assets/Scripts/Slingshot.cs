using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{

    [Header("Set in Inspector")]

    public GameObject prefabProjectile;
    private float velocityMult = 8f;

    [Header("Set Dynamically")]

    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;


    void Awake()
    {

        Transform launchPointTrans = transform.Find("LaunchPoint");              

        launchPoint = launchPointTrans.gameObject;

        launchPoint.SetActive(false);

        launchPos = launchPointTrans.position;

    }


    void OnMouseDown()
    {

        aimingMode = true;

        projectile = Instantiate(prefabProjectile) as GameObject;

        projectile.transform.position = launchPos;

        projectile.GetComponent<Rigidbody>().isKinematic = true;

        projectileRigidbody = projectile.GetComponent<Rigidbody>();

        projectileRigidbody.isKinematic = true;




    }


    void OnMouseEnter()
    {
        
        print("Slingshot: OnMouseEnter()");

        launchPoint.SetActive(true);

    }

    void OnMouseExit()
    {
        print("Slingshot: OnMouseExit()");

        launchPoint.SetActive(false);

    }

    void Update()
    {

        if (!aimingMode) return;



        // Get the current mouse position in 2D screen coordinates

        Vector3 mousePos2D = Input.mousePosition;

        mousePos2D.z = -Camera.main.transform.position.z;

        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);



        // Find the delta from the launchPos to the mousePos3D

        Vector3 mouseDelta = mousePos3D - launchPos;

        // Limit mouseDelta to the radius of the Slingshot SphereCollider          

        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude)
        {

            mouseDelta.Normalize();

            mouseDelta *= maxMagnitude;

        }

        Vector3 projPos = launchPos + mouseDelta;

        projectile.transform.position = projPos;



        if (Input.GetMouseButtonUp(0))
        {                                         // e

            // The mouse has been released

            aimingMode = false;

            projectileRigidbody.isKinematic = false;

            projectileRigidbody.velocity = -mouseDelta * velocityMult;

            projectile = null;

        }

    }
}
