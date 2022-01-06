using System;
using System.Collections.Generic;
using DataParser;
using ExoskeletonInteraction;
using MarkerDataTypes;
using TMPro;
using UI;
using UnityEngine;
using Utilities;
using ViconDataStreamSDK.CSharp;

public class LiveDataReader : MonoBehaviour
{
    public static event Action<bool, string> OnConnectionEvent;

    [Header("Connection")] 
    [SerializeField] private string defaultIP = "localhost";

    [Header("Dependencies")] 
    [SerializeField] private Transform               exoParent;
    [SerializeField] private ExoskeletonLineRenderer lr;
    [SerializeField] private Material                pointMaterial;
    [SerializeField] private TMP_Text                subjectText;

    [Header("Visual Attributes")]
    [SerializeField] private float pointSize = 0.035f;
    [SerializeField] private float bodyScale = 0.001f;

    private Client viconClient;

    private string subjectName;
    
    private List<string>                   markerNames;
    private Dictionary<string, GameObject> points;

    private void Start()
    {
        points =  new Dictionary<string, GameObject>();
        
        InputIP.OnNewIPEntered += StartLiveData;
        StartLiveData(defaultIP);
    }

    private void StartLiveData(string ip)
    {
        var connectionResult = Connect(ip);

        if(connectionResult != Result.Success)
        {
            var message = connectionResult.ToString();
            OnConnectionEvent?.Invoke(false, message);
            subjectText?.SetText($"<color=\"red\">{message}</color>");
            return;
        }

        GetInformation();
        CreatePoints();
        GetConnectsTransforms();
    }

    private Result Connect(string ip)
    {
        viconClient = new Client();
        return viconClient.Connect(ip).Result;
    }

    private void GetInformation()
    {
        markerNames = new List<string>();

        viconClient.EnableMarkerData();
        viconClient.GetFrame();

        subjectName = viconClient.GetSubjectName(0).SubjectName;
        subjectText?.SetText(subjectName);
        var markerCount = viconClient.GetMarkerCount(subjectName).MarkerCount;

        for(uint i = 0; i < markerCount; ++i)
        {
            var mName = viconClient.GetMarkerName(subjectName, i).MarkerName;
            markerNames.Add(mName);
        }
    }

    private void CleanOldPoints()
    {
        if(points is null)
            return;

        foreach(var kv in points)
            Destroy(kv.Value);

        points.Clear();
    }
    
    private void CreatePoints()
    {
        CleanOldPoints();

        foreach(var markerName in markerNames)
        {
            var go = PointCreation.CreatePoint(exoParent, pointSize, pointMaterial, markerName);
            points.Add(markerName, go);
        }
    }

    private void GetConnectsTransforms()
    {
        var connects   = ConnectionsParser.GetConnects();
        var transforms = new List<TransformConnection>();

        foreach(var (item1, item2) in connects)
        {
            var head = points[item1].transform;
            var tail = points[item2].transform;
            transforms.Add(new TransformConnection(head, tail));
        }

        lr.SetTerminals(transforms);
    }

    private void Update()
    {
        if(!viconClient.IsConnected().Connected)
            return;

        viconClient.GetFrame();
        foreach(var marker in markerNames)
        {
            var frameData = viconClient.GetMarkerGlobalTranslation(subjectName, marker);

            var x = (float)frameData.Translation[0];
            var y = (float)frameData.Translation[1];
            var z = (float)frameData.Translation[2];

            var pt = points[marker];

            if(x == y && y == z)
            {
                pt.SetActive(false);
                return;
            }
            
            pt.SetActive(true);
            pt.transform.localPosition = new Vector3(x, z, y) * bodyScale;
        }
    }
}
