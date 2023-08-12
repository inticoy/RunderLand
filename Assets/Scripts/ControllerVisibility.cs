using UnityEngine;
using System;

namespace QCHT.Interactions.Core
{
    public class ControllerVisibility : XRSubsystemLifeCycleManager<XRHandTrackingSubsystem,
        XRHandTrackingSubsystemDescriptor, XRHandTrackingSubsystem.Provider>
    {
        public XRHandTrackingSubsystem XRHTS;
        public GameObject leftHand, rightHand;
        public Action<Hands.Hand> stringEvent;        
        bool isTrackedLeft;
        bool isTrackedRight;
        int space;
        Pose rootPose;
        Pose[] joints;
        float scale;
        int gesture;
        float gestureRatio;
        float flipRatio;

        private void Start()
        {
            XRHTS = GetSubsystemInManager();
            rootPose = new Pose();
            joints = new Pose[100];
            isTrackedLeft = new bool();
            isTrackedRight = new bool();
            space = new int();
            scale = new float();
            gesture = new int();
            gestureRatio = new float();
            flipRatio = new float();
        }
        
        void Update()
        {            
            XRHTS.GetHandData(true, ref isTrackedLeft, ref space, ref rootPose, ref joints, ref scale, ref gesture, ref gestureRatio, ref flipRatio);
            XRHTS.GetHandData(false, ref isTrackedRight, ref space, ref rootPose, ref joints, ref scale, ref gesture, ref gestureRatio, ref flipRatio);           
            leftHand.SetActive(isTrackedLeft);
            rightHand.SetActive(isTrackedRight);
        }
    }
}
