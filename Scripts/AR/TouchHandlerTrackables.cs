//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google LLC">
//
// Copyright 2017 Google LLC. All Rights Reserved.
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

//using GoogleARCore;
using UnityEngine;

public class TouchHandlerTrackables
{
    /*private static TouchHandlerTrackables instance;

    public enum SpawnMode { Station, Map }
    private SpawnMode curSpawnMode;

    private bool spawnPrepared;
    private bool modeLocked;

    TrackableHitFlags raycastFilter;

    private TouchHandlerTrackables()
    {
        raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
    }

    public void HandleTouch()
    {
        if (!touchInput.isConsumed && spawnPrepared)
        {
            bool consumed = false;

            bool validTouch =
                Frame.Raycast(touchInput.touch.position.x, touchInput.touch.position.y, raycastFilter, out TrackableHit hit) &&
                GroundScanner.Instance.PlanesVisible &&
                hit.Trackable is DetectedPlane &&
                hit.Trackable.TrackingState.Equals(TrackingState.Tracking) &&
                Vector3.Dot(Camera.main.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) >= 0;

            if (validTouch)
            {
                consumed = SpawnAt(hit);
                if(consumed)
                {
                    FinishSpawn();
                }
            }
        }
    }

    public bool PrepareSpawn(SpawnMode spawnMode)
    {
        if(modeLocked)
        {
            return false;
        }

        if(HasMapSpawnPrepared()) 
        {
            NavigationMapSpawner.Instance.OnUnprepareSpawn();
        }

        curSpawnMode = spawnMode;
        spawnPrepared = true;
        GroundScanner.Instance.Activate();
        return true;
    }

    public void UnprepareMapSpawn()
    {
        if (modeLocked)
        {
            return;
        }

        spawnPrepared = false;
        GroundScanner.Instance.Deactivate();
    }

    public void FinishSpawn()
    {
        spawnPrepared = false;
        GroundScanner.Instance.Deactivate();
    }

    public bool SpawnAt(TrackableHit hit)
    {
        bool success = false;
        TrackablePose trackpose = new TrackablePose(hit.Pose, hit.Trackable);

        if (curSpawnMode.Equals(SpawnMode.Map))
        {
            bool spawnSucc = NavigationMapSpawner.Instance.SpawnAt(trackpose);
            if(spawnSucc)
            {
                // change back to station mode
                curSpawnMode = SpawnMode.Station;
            }
            success = spawnSucc;
        }
        else if (curSpawnMode.Equals(SpawnMode.Station))
        {
            Station curStation = Station.GetCurrent();
            if (curStation != null && AppManager.Instance.CurState.GetType() != typeof(StationaryState))
            {
                curStation.origin = trackpose;
                AppManager.Instance.Transition<StationaryState>();
                success = true;
            }
        }
        return success;
    }

    public bool HasMapSpawnPrepared()
    {
        return curSpawnMode.Equals(SpawnMode.Map) && spawnPrepared;
    }

    public void SetModeLock(bool isLocked)
    {
        modeLocked = isLocked;
    }*/
}
