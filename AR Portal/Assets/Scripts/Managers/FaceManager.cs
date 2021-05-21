using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class FaceManager : MonoBehaviour
{

    public Face[] faces;

    public int currentIndex = 0;

    private int maxIndex;

    bool m_IsQuitting;

    private List<AugmentedFace> _tempAugmentedFaces = new List<AugmentedFace>();

    public ScanningAnimation scanningAnimation;

    private void Start()
    {
        maxIndex = faces.Length - 1;

        SwitchFace();
    }

    public void SwitchFace()
    {
        currentIndex += 1;

        if (currentIndex > maxIndex)
        {
            currentIndex = 0;
        }

        ChangeFace();
    }

    private void ChangeFace()
    {
        for (int i = 0; i < faces.Length; i++)
        {
           faces[i].gameObject.SetActive(i == currentIndex ? true : false);

            if (i == currentIndex)
                scanningAnimation.SetIcon(faces[i].faceIcon);
        }
    }

    void Update()
    {
        _UpdateApplicationLifecycle();

        Session.GetTrackables<AugmentedFace>(_tempAugmentedFaces, TrackableQueryFilter.All);

        if(_tempAugmentedFaces.Count == 0)
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            faces[currentIndex].gameObject.SetActive(false);
            //Don't move scanning image if there's no face
            scanningAnimation.MoveIcon(false);
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            faces[currentIndex].gameObject.SetActive(true);
            scanningAnimation.MoveIcon(true);
        }
        
    }

    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
    private void _UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (m_IsQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void _DoQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer =
            new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>(
                    "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }

}
