using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [HideInInspector] public bool isEnabled = true;
    [HideInInspector] public Camera _camera;
    private float _xRotation = 0f;

    [HideInInspector] public Vector2 mouseInput;

    [HideInInspector] public float playerCamDistance;
    private float _targetDistance;
    private float _currDistance;

    [HideInInspector] public float _turnSpeed;

    [HideInInspector] public float _minTurnAngle;
    [HideInInspector] public float _maxTurnAngle;


    public void OnLook(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                mouseInput = context.ReadValue<Vector2>();
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Canceled:
                mouseInput = Vector2.zero;
                break;
        }
    }

    public void ProcessLook()
    {

        float y = mouseInput.x * _turnSpeed;
        _xRotation += mouseInput.y * _turnSpeed;
        _xRotation = Mathf.Clamp(_xRotation, _minTurnAngle, _maxTurnAngle);

        //apply to camera
        _camera.transform.localRotation = Quaternion.Euler(-_xRotation, _camera.transform.eulerAngles.y + y, 0);

        // Draw a ray from
        RaycastHit hit;
        int mask = LayerMask.GetMask("Walls") + LayerMask.GetMask("MapInvisibleWalls");

        Vector3 cameraDirection = (-_camera.transform.forward * 2.0f + _camera.transform.up * 0.0f).normalized;

        if (Physics.Raycast(transform.position,  cameraDirection, out hit, _targetDistance, mask))
        {
            float highDist = hit.distance;
            float lowDist = 0f;
            float midDist = (highDist - lowDist) / 2 + lowDist;
            float cameraRadius = 0.2f;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position + cameraDirection * midDist, cameraRadius, mask);

            for (int i = 0; i < 10; i++)
            {
                if (hitColliders.Length > 0) // If we hit something, move closer to origin
                {
                    // Debug.Log(hitColliders[0].gameObject.layer);
                    highDist = midDist;
                }
                else // We hit nothing, move further from origin
                {
                    lowDist = midDist;
                }
                midDist = (highDist - lowDist) / 2 + lowDist;
                hitColliders = Physics.OverlapSphere(transform.position + cameraDirection * midDist, cameraRadius, mask);
            }
//            Debug.Log(transform.position + cameraDirection * highDist);
            _currDistance = highDist;

            //Debug.DrawRay(transform.position, -_camera.transform.forward * hit.distance, Color.yellow);
        }
        else
        {
            // Debug.DrawRay(transform.position, -_camera.transform.forward * _targetDistance, Color.yellow);
            _currDistance = _targetDistance;
        }

        // rotate player
        //transform.Rotate(Vector3.up * (mouseInput.x * Time.deltaTime) * xSensitivity);
        // Vector3 floorOffset = new Vector3();
        // if (_xRotation > 0){
        //     floorOffset = Vector3.up * 0.2f;
        // }

        //
        _camera.transform.position = transform.position + cameraDirection * _currDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _targetDistance = playerCamDistance;
        //_camera.transform.position = transform.position + new Vector3(PLAYER_CAM_DISTANCE, 0, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isEnabled)
            ProcessLook();
    }
}