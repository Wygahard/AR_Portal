using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARController : MonoBehaviour
{
    //TrackedPlane has been depreciated for DetectedPlane
    private List<DetectedPlane> m_NewTrackedPlanes = new List<DetectedPlane>();
    public GameObject GridPrefab;
    public GameObject Portal;
    public GameObject ARCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check ARCore session status
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        //Will fill m_NewTrackedPlanes with the planes detected by ARCore every frame
        Session.GetTrackables<DetectedPlane>(m_NewTrackedPlanes, TrackableQueryFilter.New);

        //Instantiate a Grid for each detected plane
        for (int i = 0; i < m_NewTrackedPlanes.Count; i++)
        {
            GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);
            //Set position of grid and modify the vertices of attached mesh
            grid.GetComponent<GridVisualiser>().Initialize(m_NewTrackedPlanes[i]);
        }

        //Check if the user touches the screen
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        //Check if user touched any of the tracked planes
        TrackableHit hit;
        if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit))
        {
            //Place portal
            Portal.SetActive(true);
            //Create new Anchor
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);
            //Set portal position
            Portal.transform.position = hit.Pose.position;
            Portal.transform.rotation = hit.Pose.rotation;

            //Portal face the camera
            Vector3 cameraPosition = ARCamera.transform.position;
            //Rotate on Y axis
            cameraPosition.y = hit.Pose.position.y;
            Portal.transform.LookAt(cameraPosition, Portal.transform.up);
            //ARCore will update anchors accordingly
            Portal.transform.parent = anchor.transform;
        }
    }
}
