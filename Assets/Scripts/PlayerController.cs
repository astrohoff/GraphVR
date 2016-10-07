using UnityEngine;
using System.Collections;
using UnityEngine.VR;
public class PlayerController : MonoBehaviour
{
    public float angle_padding, look_speed, moveSpeed;
    float rot_x, rot_y;
    Quaternion init_rot;
    bool ignoreMouseToggle;
    void Start()
    {
        init_rot = transform.localRotation;
        
    }
    void Update()
    {
        if (VRSettings.enabled && Input.GetButtonDown("Recenter"))
        {
            InputTracking.Recenter();
        }
        if(Input.GetButtonDown("Ignore Mouse")) {
            ignoreMouseToggle = !ignoreMouseToggle;
        }
        if (!VRSettings.enabled && !ignoreMouseToggle)
        {
            // mouselook
            rot_x += Input.GetAxis("Mouse X") * Time.deltaTime * look_speed;
            rot_x = ClampAngle(rot_x, -360, 360);
            rot_y += Input.GetAxis("Mouse Y") * Time.deltaTime * look_speed;
            rot_y = ClampAngle(rot_y, -90 + angle_padding, 90 - angle_padding);
            Quaternion q_rot_x = Quaternion.AngleAxis(rot_x, Vector3.up);
            Quaternion q_rot_y = Quaternion.AngleAxis(rot_y, -Vector3.right);
            transform.localRotation = init_rot * q_rot_x * q_rot_y;
            // free camera movement
            Vector3 posMove = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
            posMove = posMove.normalized * moveSpeed * Time.deltaTime;
            transform.position += posMove;
        }
    }
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        else if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}