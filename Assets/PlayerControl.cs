using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class PlayerControl : MonoBehaviour
{

    private bool jump;

    private Rigidbody rb;

    private bool isGrounded;



    // Use this for initialization

    void Start()
    {

        rb = this.gameObject.GetComponent<Rigidbody>();



    }

    void FixedUpdate()

    {

        if (jump)
        {

            jump = false;

            rb.AddForce(0, this.gameObject.GetComponent<Rigidbody>().mass * 8, 0, ForceMode.Impulse);

        }



    }

    void OnCollisionStay()

    {

        isGrounded = true;

    }

    // Update is called once per frame

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)

            jump = true;

        isGrounded = false;



    }

}
