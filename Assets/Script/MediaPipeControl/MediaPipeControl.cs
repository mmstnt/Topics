using Google.Protobuf.WellKnownTypes;
using Mediapipe;
using Mediapipe.Tasks.Vision.FaceLandmarker;
using Mediapipe.Unity.CoordinateSystem;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaPipeControl : MonoBehaviour
{
    public FaceLandmarkerRunner _faceLandmarkerRunner;
    public List<Vector3> faceLandmarkList;
    public Camera _camera;
    public List<GameObject> pointGameObjectList;
    public GameObject pointGameObject;
    private void Start()
    {
        _faceLandmarkerRunner = this.GetComponent<FaceLandmarkerRunner>();
        faceLandmarkList = new List<Vector3>();

        for(int i = 0; i < 478; i++) 
        {
            pointGameObjectList.Add(Instantiate(pointGameObject, new Vector3(0, 0, 0), Quaternion.identity,GameObject.Find("Game/Game Camera").transform));
            faceLandmarkList.Add(new Vector3(0, 0, 0));
        }

        _faceLandmarkerRunner.callBackFaceLandmark += result =>
        {
            FaceLandmarkerResult faceLandmarResult = result;
            if (faceLandmarResult.faceLandmarks is null) return;

            var faceLandmarks = faceLandmarResult.faceLandmarks[0];
            if (faceLandmarkList.Count != 0) 
            {
                for (int i = 0; i < faceLandmarks.landmarks.Count; i++)
                {
                    var faceLandmark = faceLandmarks.landmarks[i];
                    faceLandmarkList[i] = new Vector3(faceLandmark.x, faceLandmark.y, faceLandmark.z);
                }
            }
            else 
            {
                for (int i = 0; i < faceLandmarks.landmarks.Count; i++)
                {
                    var faceLandmark = faceLandmarks.landmarks[i];
                    faceLandmarkList.Add(new Vector3(faceLandmark.x, faceLandmark.y, faceLandmark.z));
                }
            }
        };
    }

    private void Update()
    {
        for (int i = 0; i < pointGameObjectList.Count; i++)
        {
            Vector3 site = _camera.ViewportToWorldPoint(new Vector3(faceLandmarkList[i].x, faceLandmarkList[i].y, faceLandmarkList[i].z));
            site.y = -site.y + 2*_camera.transform.position.y;
            pointGameObjectList[i].transform.position = site;
        }
    }

}
