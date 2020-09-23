//-----------------------------------------------------------------------
// <copyright file="AugmentedImageExampleController.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
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

/*
using System;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;

public class CustomAugImageController : MonoBehaviour
{
    public delegate void OnAnchorFound(Anchor anchor);

    public static CustomAugImageController Instance { get; private set; }
    private bool isActive;

    public GameObject ScanCue;
    public Transform moveToAnchor;
    public GroundScanner groundScanner;

    private List<AugmentedImage> tempAugmentedImages = new List<AugmentedImage>();
    private int imgCount = -1;

    //private bool activate_asa_tracking = false;
    private bool searchForImage;
    
    private AugmentedImage curImage;
    private string searchImageName;

    private OnAnchorFound onImageFound;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        // Enable ARCore to target 60fps camera capture frame rate on supported devices.
        // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        if (!MobileOnlyActivator.IsMobile)
            Destroy(this);
    }

    public void Update()
    {
        if (!isActive)
            return;
        // Get updated augmented images for this frame.
        Session.GetTrackables<AugmentedImage>(tempAugmentedImages, TrackableQueryFilter.Updated);

        //if there are more or less images than last frame
        if (searchForImage && imgCount != tempAugmentedImages.Count) {
            foreach (AugmentedImage image in tempAugmentedImages)
            {
                if (image.Name.Equals(searchImageName))
                {
                    //only setting flag so that the activation begins as soon as the image is in state "tracking", not before
                    curImage = image;
                    
                    //setting cur image to first image, duh
                    searchForImage = false;
                    //onImageFound?.Invoke()
                    ScanCue.SetActive(false);
                }
            }
            imgCount = tempAugmentedImages.Count;
        }

        // if image is tracking but no anchor has been set
        if (imgCount > 0 && curImage != null && curImage.TrackingState == TrackingState.Tracking && !searchForImage)
        {
            onImageFound?.Invoke(curImage.CreateAnchor(curImage.CenterPose));
            //if (groundScanner == null)
            //    return;

            // use anchor on closest plane instaed of anchor on image
            // for better tracking
            //Anchor anchor = groundScanner.ClosestAnchorOnPlane(curImage.CenterPose);
            //Anchor cur_anchor = curImage.CreateAnchor(curImage.CenterPose);
            //if (anchor != null)
            //{
            //    onAnchorFound?.Invoke(anchor);
            //    Deactivate();
            //}
        }
    }
    
    public void SearchForImage(string imageName, OnAnchorFound onImageFound)
    {
        isActive = true;
        searchForImage = true;
        this.searchImageName = imageName;
        this.onImageFound = onImageFound;

        ScanCue.SetActive(true);
        groundScanner?.Activate();
    }
    
    public void Deactivate()
    {
        isActive = false;
        ScanCue.SetActive(false);
        imgCount = 0;

        curImage = null;
        searchImageName = string.Empty;
        onImageFound = null;
        
        tempAugmentedImages.Clear();
        groundScanner?.Deactivate();
    }
} */