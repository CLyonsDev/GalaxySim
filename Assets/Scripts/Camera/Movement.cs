using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    float speedFB = 4f;
    float speedLR = 4f;
    float speedUD = 5.5f;

    float minSpeed = 1f;
    float maxSpeed = 7f;
    float speedIncrement = 0.5f;

    float lerpSpeed = 15f;

    float upDn = 0;

	void Start () {
		
	}
	
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && speedFB < maxSpeed)
        {
            speedFB += speedIncrement;
            speedLR += speedIncrement;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && speedFB > minSpeed)
        {
            speedFB -= speedIncrement;
            speedLR -= speedIncrement;
        }
    }

	void FixedUpdate () {
        //Calculate horizontal speed because unity axis manager does not like me rn.
        upDn = Mathf.Lerp(upDn, Input.GetAxis("UpDown"), 200 * Time.fixedDeltaTime);

        Vector3 forward = transform.forward * speedFB * Input.GetAxis("Vertical");
        Vector3 right = transform.right * speedLR * Input.GetAxis("Horizontal");
        Vector3 up = Vector3.up * speedUD * upDn;

        Vector3 targetDest = transform.position + (forward + right + up);

        transform.position = Vector3.Slerp(transform.position, targetDest, lerpSpeed * Time.fixedDeltaTime);
    }
}
