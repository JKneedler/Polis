using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private float moveSpeed;
    public float minMoveSpeed;
    public float maxMoveSpeed;
    public float rotateSpeed;
    public float pitchSpeed;

    public Transform target;
    private float distance = 5.0f;
    public float xSpeed;
    public float ySpeed;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;
    private Quaternion newRotation;
    public Quaternion startingRotation;

    public Vector3 startingDragMousePos;
    public Vector3 curDragMousePos;
    public Vector3 mousePosDifference;
    private Vector3 targetOriginalPos;
    public float dragScale;
    private Vector3 relativePos;
    public int dragLayerMask = 1 << 9;

    float x = 0.0f;
    float y = 0.0f;

    // Start is called before the first frame update
    void Start() {
      newRotation = startingRotation;
      Vector3 angles = transform.eulerAngles;
      x = angles.y;
      y = angles.x;

    }

    // Update is called once per frame
    void Update() {

      if (Input.GetMouseButton(1)) {
          x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
          y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

          y = ClampAngle(y, yMinLimit, yMaxLimit);

          newRotation = Quaternion.Euler(y, x, 0);
      }
      SetCameraPosition();
      moveSpeed = (distance / distanceMax) * (maxMoveSpeed - minMoveSpeed) + minMoveSpeed;

      if(Input.GetButton("W")){
        target.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
      }

      if(Input.GetButton("S")){
        target.Translate(Vector3.back * Time.deltaTime * moveSpeed);
      }

      if(Input.GetButton("A")){
        target.Translate(Vector3.left * Time.deltaTime * moveSpeed);
      }

      if(Input.GetButton("D")){
        target.Translate(Vector3.right * Time.deltaTime * moveSpeed);
      }

      if(Input.GetMouseButtonDown(2)) {
        targetOriginalPos = target.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, dragLayerMask)) {
          startingDragMousePos = hit.point;
          mousePosDifference = new Vector3(0, 0, 0);
        }
      }

      if(Input.GetMouseButton(2)) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, dragLayerMask)) {
          curDragMousePos = hit.point;
          mousePosDifference = startingDragMousePos - curDragMousePos;
          mousePosDifference = new Vector3(mousePosDifference.x, 0, mousePosDifference.z);
          target.position += mousePosDifference;
        }
      }

    }

    void LateUpdate() {

    }

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void SetCameraPosition() {
      distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);

      Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
      Vector3 position = newRotation * negDistance + target.position;

      transform.rotation = newRotation;
      transform.position = position;
      relativePos = transform.position - target.position;
      relativePos = new Vector3(relativePos.x, 0, relativePos.z);
      Quaternion newTargetRot = Quaternion.LookRotation(-relativePos, Vector3.up);
      target.rotation = newTargetRot;
    }
}
