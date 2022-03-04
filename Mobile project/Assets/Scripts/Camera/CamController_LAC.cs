/*

Set this on an empty game object positioned at (0,0,0) and attach your active camera.
The script only runs on mobile devices or the remote app.

*/

using UnityEngine;

class CamController_LAC : MonoBehaviour
{

    public Camera camera;
    public bool rotate;
    
    protected Plane plane;
    public Vector3 clampArea;
    private void Awake()
    {
        if (camera == null)
            camera = Camera.main;
    }

    private void Update()
    {

        //Update Plane
        if (Input.touchCount >= 1)
            plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll
        if (Input.touchCount >= 1)
        {
            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                camera.transform.Translate(Delta1, Space.World);
        }

        //Pinch
        if (Input.touchCount >= 2)
        {
            var pos1  = PlanePosition(Input.GetTouch(0).position);
            var pos2  = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);
            
            //edge case
            if (zoom == 0 || zoom > 10)
                return;

            //Move cam amount the mid ray
            Vector3 newCamPos = Vector3.LerpUnclamped(pos1, camera.transform.position, 1 / zoom);

            // clamp cam zoom
            if (plane.GetDistanceToPoint(newCamPos) < clampArea.y*2 && plane.GetDistanceToPoint(newCamPos)>1)
                camera.transform.position = newCamPos;

            if (rotate && pos2b != pos2)
                camera.transform.RotateAround(pos1, plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, plane.normal));
        }

        // clamp
        if (camera.transform.position.x > clampArea.x)
            camera.transform.position = new Vector3(clampArea.x, camera.transform.position.y, camera.transform.position.z);

        if (camera.transform.position.x < -clampArea.x)
            camera.transform.position = new Vector3(-clampArea.x, camera.transform.position.y, camera.transform.position.z);

        if (camera.transform.position.z < -clampArea.z)
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -clampArea.z);

        if (camera.transform.position.z > clampArea.z)
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, clampArea.z);



    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = camera.ScreenPointToRay(touch.position);
        if (plane.Raycast(rayBefore, out var enterBefore) && plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }


    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = camera.ScreenPointToRay(screenPos);
        if (plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);

        Gizmos.DrawWireCube(transform.position + Vector3.up * (clampArea.y+1), clampArea * 2);
    }

}