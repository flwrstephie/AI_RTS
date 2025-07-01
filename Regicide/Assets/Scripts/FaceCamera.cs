using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCam != null)
        {
            // Face the camera, but keep upright orientation
            Vector3 lookDirection = transform.position - mainCam.transform.position;
            lookDirection.y = 0; // optional: keep it upright
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}
