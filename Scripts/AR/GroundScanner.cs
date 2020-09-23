//-----------------------------------------------------------------------
// <copyright file="DetectedPlaneGenerator.cs" company="Google LLC">
//
// Copyright 2018 Google LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
/*using System;
using System.Collections.Generic;
using GoogleARCore;
//using GoogleARCore.Examples.Common;
using UnityEngine;

public class GroundScanner : MonoBehaviour
{
    public static GroundScanner Instance { get; private set; }

    public ARCoreSessionConfig config;
    public DetectedPlaneFindingMode activeFindingMode;

    public GameObject DetectedPlanePrefab;
    private List<DetectedPlane> newPlanes = new List<DetectedPlane>();
    private TrackableHitFlags raycastFilter;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        // if there already is an instance of appmanager
        // destory this one
        else
        {
            Destroy(this);
        }
        raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
    }

    void Start()
    {
        Deactivate();
    }

    public void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
            return;

        Session.GetTrackables<DetectedPlane>(newPlanes, TrackableQueryFilter.New);
        for (int i = 0; i < newPlanes.Count; i++)
        {
            GameObject planeObject = 
                Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
            planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(newPlanes[i]);
        }
    }

    public void Activate()
    {
        config.PlaneFindingMode = activeFindingMode;
    }
    public void Deactivate()
    {
        config.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
    }

    public Anchor ClosestAnchorOnPlane(Pose pose)
    {
        Vector3[] directions = new Vector3[]
        {
            pose.right, pose.right*-1, pose.forward, pose.forward*-1, pose.up, pose.up*-1
        };

        float minDistance = float.MaxValue;
        bool hitSomething = false;
        (Trackable trackable, Pose pose) trackPose = default;

        for(int i = 0; i < directions.Length; i++)
        {
            // raycast in all directions
            if(Frame.Raycast(pose.position, directions[i], out TrackableHit hit, float.PositiveInfinity, raycastFilter)
                && HitIsValid(hit))
            {
                // return anchor on hit plane
                if(hit.Distance < minDistance)
                {
                    hitSomething = true;
                    trackPose = (hit.Trackable, hit.Pose);
                }
            }
        }

        if (hitSomething)
            return trackPose.trackable.CreateAnchor(trackPose.pose);
        else
            return null;
    }

    private bool HitIsValid(TrackableHit hit)
    {
        return hit.Trackable is DetectedPlane &&
               hit.Trackable.TrackingState.Equals(TrackingState.Tracking);
        //Vector3.Dot(Camera.main.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) >= 0;
    }
} */