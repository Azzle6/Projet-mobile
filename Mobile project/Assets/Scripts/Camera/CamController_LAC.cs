/*

Set this on an empty game object positioned at (0,0,0) and attach your active camera.
The script only runs on mobile devices or the remote app.

*/

using UnityEngine;

class CamController_LAC : MonoBehaviour
{

    public Camera camera;
    public bool rotate;
    
    public Plane plane;

    public float maxZoom = 5;
    public float clampRadius = 3;

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
        if (Input.touchCount == 1)
        {
            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector3 camDelta1 = lookPos() +Delta1;
                if (Vector3.Distance(camDelta1, transform.position) > clampRadius)
                    Delta1 = Vector3.zero;

                camera.transform.position += Delta1;
            }
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
            float t = Mathf.Clamp(1 / zoom, 0.2f, maxZoom / Vector3.Distance(lookPos(), camera.transform.position));
            Vector3 newCamPos = Vector3.LerpUnclamped(lookPos(), camera.transform.position,t );
            //Debug.Log("Cam dist : " + Vector3.Distance(lookPos(), camera.transform.position));
            // clamp cam zoom
            if (plane.GetDistanceToPoint(newCamPos) >= 1)
                camera.transform.position = newCamPos;

            if (rotate && pos2b != pos2)
                camera.transform.RotateAround(lookPos(), plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, plane.normal));
        }

    }

    protected Vector3 lookPos()
    {
        return PlanePosition(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
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
        Vector3 lookPosG = (lookPos() == Vector3.zero) ? transform.position : lookPos();

        Gizmos.color = Color.green;
        Gizmos.DrawRay(lookPosG, (camera.transform.position - lookPosG).normalized * maxZoom);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lookPosG,0.2f);
        Gizmos.DrawLine(camera.transform.position, lookPosG);
        Gizmos.DrawWireSphere(transform.position, clampRadius);

    }

}