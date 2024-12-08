using Google.Protobuf.WellKnownTypes;
using Mediapipe;
using Mediapipe.Tasks.Vision.FaceLandmarker;
using Mediapipe.Unity.CoordinateSystem;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using OpenCvSharp;
using System;

public class MediaPipeControl : MonoBehaviour
{
    public FaceLandmarkerRunner _faceLandmarkerRunner;
    public List<Vector3> faceLandmarkList;
    public Camera _camera;
    public List<GameObject> pointGameObjectList;
    public GameObject pointGameObject;
    public GameObject leftEve;
    public GameObject rightEve;
    public GameObject testgame;
    public float test;

    private Vector3 previousDirection = Vector3.zero;
    private Vector3 lastFixedDirection = Vector3.zero;

    private void Start()
    {
        _faceLandmarkerRunner = this.GetComponent<FaceLandmarkerRunner>();
        faceLandmarkList = new List<Vector3>();

        for (int i = 0; i < 478; i++)
        {
            GameObject g = Instantiate(pointGameObject, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Game Camera").transform);
            g.name = i.ToString();
            pointGameObjectList.Add(g);
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
                    faceLandmarkList[i] = SmoothKeyPoint(new Vector3(faceLandmark.x, faceLandmark.y, faceLandmark.z),faceLandmarkList[i]);
                    //faceLandmarkList[i] = new Vector3(faceLandmark.x, faceLandmark.y, faceLandmark.z);
                }
            }
        };
    }

    private void Update()
    {

        (var rightDirection, var leftDirection) = Gaze(1920, 1080, faceLandmarkList, previousDirection);
        Vector3 cameraForward = _camera.transform.forward;
        rightDirection = FixDirection(rightDirection, cameraForward);
        leftDirection = FixDirection(leftDirection, cameraForward);
        for (int i = 0; i < pointGameObjectList.Count; i++)
        {
            pointGameObjectList[i].transform.position = getPointSite(i);
        }
        
        if (rightDirection != Vector3.zero)
        {
            Quaternion rightEyeRotation = Quaternion.LookRotation(rightDirection);
            pointGameObjectList[473].transform.rotation = rightEyeRotation;
            //rightEve.transform.position = (pointGameObjectList[33].transform.position + pointGameObjectList[173].transform.position) / 2 - pointGameObjectList[468].transform.rotation * Vector3.forward * test;
            //rightEve.transform.LookAt(pointGameObjectList[468].transform);
        }
        if (leftDirection != Vector3.zero)
        {
            Quaternion leftEyeRotation = Quaternion.LookRotation(leftDirection);
            pointGameObjectList[468].transform.rotation = leftEyeRotation;
            //leftEve.transform.position = (pointGameObjectList[362].transform.position + pointGameObjectList[263].transform.position) / 2 - pointGameObjectList[473].transform.rotation * Vector3.forward * test;
            //leftEve.transform.LookAt(pointGameObjectList[473].transform);
        }

        
        
        testgame.transform.position = Vector3.Lerp(pointGameObjectList[473].GetComponent<MediaPipeRay>().hit, pointGameObjectList[468].GetComponent<MediaPipeRay>().hit, 0.5f);

        //testgame.transform.position = (pointGameObjectList[468].GetComponent<MediaPipeRay>().hit + pointGameObjectList[473].GetComponent<MediaPipeRay>().hit) / 2;
    }

    private Vector3 getPointSite(int i)
    {
        Vector3 site = _camera.ViewportToWorldPoint(new Vector3(faceLandmarkList[i].x, faceLandmarkList[i].y, faceLandmarkList[i].z));
        //site.x = site.x;
        site.y = -site.y + 2 * _camera.transform.position.y;
        site.z = -site.z * 30 + 32 * _camera.transform.position.z;
        return site;
    }

    Vector3 SmoothKeyPoint(Vector3 newPoint, Vector3 prevPoint, float smoothingFactor = 0.5f)
    {
        return Vector3.Lerp(prevPoint, newPoint, 1 - smoothingFactor);
    }

    Vector3 FixDirection(Vector3 direction, Vector3 cameraForward, float smoothingFactor = 0.5f, float stabilityThreshold = 0.05f)
    {
        Vector3 worldDirection = _camera.transform.TransformDirection(direction);

        if (Vector3.Dot(worldDirection, cameraForward) > 0)
        {
            float yawOffset = Mathf.Sin(Mathf.Deg2Rad * _camera.transform.eulerAngles.y);
            worldDirection.x += yawOffset * 0.1f;
        }
        else
        {
            worldDirection = -worldDirection;
        }

        worldDirection.Normalize();

        // 平滑處理
        if (Vector3.Distance(lastFixedDirection, worldDirection) > stabilityThreshold)
        {
            lastFixedDirection = Vector3.Lerp(lastFixedDirection, worldDirection, 1 - smoothingFactor);
        }

        return lastFixedDirection;
    }

    public static (Vector3 rightEyeDirection, Vector3 leftEyeDirection) Gaze(int frameHeight, int frameWidth, List<Vector3> faceLandmarkList, Vector3 previousDirection, float smoothingFactor = 0.5f, float stabilityThreshold = 0.02f)
    {
        // 2D 圖像點
        double[,] imagePoints = new double[6, 2]
        {
        { faceLandmarkList[4].x * frameWidth, faceLandmarkList[4].y * frameHeight },        // 鼻尖
        { faceLandmarkList[152].x * frameWidth, faceLandmarkList[152].y * frameHeight },    // 下巴
        { faceLandmarkList[263].x * frameWidth, faceLandmarkList[263].y * frameHeight },    // 左眼左角
        { faceLandmarkList[33].x * frameWidth, faceLandmarkList[33].y * frameHeight },      // 右眼右角
        { faceLandmarkList[287].x * frameWidth, faceLandmarkList[287].y * frameHeight },    // 左嘴角
        { faceLandmarkList[57].x * frameWidth, faceLandmarkList[57].y * frameHeight }       // 右嘴角
        };

        // 3D 模型點
        double[,] modelPoints = new double[6, 3]
        {
        { 0.0, 0.0, 0.0 },       // 鼻尖
        { 0, -63.6, -12.5 },     // 下巴
        { -43.3, 32.7, -26 },    // 左眼左角
        { 43.3, 32.7, -26 },     // 右眼右角
        { -28.9, -28.9, -24.1 }, // 左嘴角
        { 28.9, -28.9, -24.1 }   // 右嘴角
        };

        // 3D 模型眼睛點 (根據faceLandmarkList中的3D座標)
        double[] eyeBallCenterRight = new double[3] {
        faceLandmarkList[473].x, // 右眼眼球中心的 x
        faceLandmarkList[473].y, // 右眼眼球中心的 y
        faceLandmarkList[473].z  // 右眼眼球中心的 z
        };

        double[] eyeBallCenterLeft = new double[3] {
        faceLandmarkList[468].x, // 左眼眼球中心的 x
        faceLandmarkList[468].y, // 左眼眼球中心的 y
        faceLandmarkList[468].z  // 左眼眼球中心的 z
        };


        // 相機內參矩陣
        double focalLength = frameWidth;
        var center = (frameWidth / 2.0, frameHeight / 2.0);
        double[,] cameraMatrix = new double[,] {
        { focalLength, 0, center.Item1 },
        { 0, focalLength, center.Item2 },
        { 0, 0, 1 }
        };

        double[,] distCoeffs = new double[5, 1] { { 0 }, { 0 }, { 0 }, { 0 }, { 0 } }; // 使用 5 個參數避免不穩定

        Mat rvec = new Mat(), tvec = new Mat();
        Mat modelPointsMat = Mat.FromArray(modelPoints);
        Mat imagePointsMat = Mat.FromArray(imagePoints);
        Mat cameraMatrixMat = Mat.FromArray(cameraMatrix);
        Mat distCoeffsMat = Mat.FromArray(distCoeffs);

        Cv2.SolvePnP(modelPointsMat, imagePointsMat, cameraMatrixMat, distCoeffsMat, rvec, tvec, flags: SolvePnPFlags.Iterative);

        // 計算右眼方向
        Mat eyeBallCenterRightMat = Mat.FromArray(eyeBallCenterRight);
        Mat rightDirectionVector = new Mat();
        Cv2.Subtract(eyeBallCenterRightMat, tvec, rightDirectionVector);
        Cv2.Normalize(rightDirectionVector, rightDirectionVector);

        // 計算左眼方向
        Mat eyeBallCenterLeftMat = Mat.FromArray(eyeBallCenterLeft);
        Mat leftDirectionVector = new Mat();
        Cv2.Subtract(eyeBallCenterLeftMat, tvec, leftDirectionVector);
        Cv2.Normalize(leftDirectionVector, leftDirectionVector);

        // 提取方向向量
        Vector3 ExtractDirection(Mat directionVector)
        {
            Vector3 currentDirection = new Vector3(
                (float)directionVector.At<double>(0, 0),
                (float)directionVector.At<double>(1, 0),
                (float)directionVector.At<double>(2, 0)
            );

            // 平滑過濾
            if ((currentDirection - previousDirection).sqrMagnitude > stabilityThreshold)
            {
                previousDirection = Vector3.Lerp(previousDirection, currentDirection, 1 - smoothingFactor);
            }

            return previousDirection.normalized;
        }

        Vector3 rightEyeDirection = ExtractDirection(rightDirectionVector);
        Vector3 leftEyeDirection = ExtractDirection(leftDirectionVector);

        rightEyeDirection.y = -rightEyeDirection.y;
        leftEyeDirection.y = -leftEyeDirection.y;

        return (rightEyeDirection, leftEyeDirection);
    }
}
