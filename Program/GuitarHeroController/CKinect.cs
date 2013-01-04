using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using Microsoft.Kinect;


namespace GuitarHeroController
{
    public class CKinect
    {
        #region Attributes
        private KinectSensor _kinect;
        private double _leftX;
        private double _leftY;
        private double _rightX;
        private double _rightY;
        private bool _tracked;
        private bool _leftTracked;
        private bool _rightTracked;
        private bool _isStart;
        private Skeleton[] _skeletons;

        /// <summary>
        /// the value to check if someone is in front of the kinect;
        /// true: someone is;
        /// false:someone is not.
        /// </summary>
        public bool Tracked {
            get { return this._tracked; }
        }
        public bool LeftTracked {
            get { return this._leftTracked; }
        }
        public bool RightTracked
        {
            get { return this._rightTracked; }
        }
        public bool isStart {
            get { return this._isStart; }
        }
        #endregion
        public CKinect()
        {
            initializeKinect();
        }
        #region Kinect Operation
        private void initializeKinect()
        {
            this._tracked = false;
            this._leftTracked = false;
            this._rightTracked = false;
            this._isStart = false;

            foreach (var potentialKinect in KinectSensor.KinectSensors)
            {
                if (potentialKinect.Status == KinectStatus.Connected)
                {
                    this._kinect = potentialKinect;
                    this._skeletons = new Skeleton[this._kinect.SkeletonStream.FrameSkeletonArrayLength];
                    break;
                }
            }
            if (this._kinect != null)
            {
                TransformSmoothParameters parameters = new TransformSmoothParameters();
                parameters.Smoothing = 0.5f;
                parameters.Correction = 0.5f;
                parameters.Prediction = 0.5f;
                parameters.JitterRadius = 0.05f;
                parameters.MaxDeviationRadius = 0.04f;
                this._isStart = false;
                this._kinect.SkeletonStream.Enable(parameters);
            }
        }
        /// <summary>
        /// start kinect and make ready for skeleton tracking
        /// </summary>
        /// <returns></returns>
        public Boolean start()
        {
            try
            {
                if (this._kinect != null) {
                    this._kinect.SkeletonFrameReady += _kinect_SkeletonFrameReady;
                    this._kinect.Start();
                }
            }
            catch (IOException)
            {
                this._kinect = null;
            }
            if (this._kinect != null)
            {
                this._isStart = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void stop()
        {
            try
            {
                if (this._kinect != null && this._isStart) {
                    this._kinect.SkeletonFrameReady -= _kinect_SkeletonFrameReady;
                    this._kinect.Stop();
                    this._isStart = false;
                }
            }
            catch (IOException)
            {
                this._kinect = null;
            }
        }

        void _kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            //Skeleton[] skeletons = new Skeleton[0];
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame()) // Open the Skeleton frame
            {
                if (skeletonFrame != null && this._skeletons != null) // check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(this._skeletons); // get the skeletal information in this frame
                }
            }

            if ((this._skeletons.Length != 0) && this._skeletons != null)
            {
                Skeleton skeleton = new Skeleton();
                foreach (Skeleton s in this._skeletons)
                {
                    if (s != null && s.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        skeleton = s;
                        break;
                    }
                }
                /*Skeleton skeleton = (from s in this._skeletons
                                     where s.TrackingState == SkeletonTrackingState.Tracked
                                     select s).FirstOrDefault();*/
                if (skeleton != null)
                {
                    this._tracked = true;
                    if (skeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked)
                    {
                        this._leftTracked = true;
                        SkeletonPoint handLeftPt = skeleton.Joints[JointType.HandLeft].Position;
                        this._leftX = handLeftPt.X;
                        this._leftY = handLeftPt.Y;
                    }
                    else
                    {
                        this._leftTracked = false;
                    }
                    if (skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
                    {
                        this._rightTracked = true;
                        SkeletonPoint handRightPt = skeleton.Joints[JointType.HandRight].Position;
                        this._rightX = handRightPt.X;
                        this._rightY = handRightPt.Y;
                    }
                    else
                    {
                        this._rightTracked = false;
                    }
                }
                else
                {
                    this._tracked = false;
                }
            }
            else {
                this._tracked = false;
                this._leftTracked = false;
                this._rightTracked = false;
            }
        }
        #endregion
        #region Kinect Data
        /// <summary>
        /// get the position of the hand
        /// </summary>
        /// <returns></returns>
        public double getLeftShadowX()
        {
            return this._leftX;
        }
        public double getLeftShadowY()
        {
            return this._leftY;
        }

        public double getRightShadowX()
        {
            return this._rightX;
        }
        public double getRightShadowY()
        {
            return this._rightY;
        }
        /// <summary>
        /// adjust the elevation angle
        /// of the kinect
        /// </summary>
        public int ElevationAngle
        {
            get { return this._kinect.ElevationAngle; }
            set
            {
                if (this._kinect != null && this.isStart) {
                    if (value < this._kinect.MinElevationAngle)
                    {
                        this._kinect.ElevationAngle = this._kinect.MinElevationAngle;
                    }
                    else if (this._kinect.MaxElevationAngle < value)
                    {
                        this._kinect.ElevationAngle = this._kinect.MaxElevationAngle;
                    }
                    else
                    {
                        this._kinect.ElevationAngle = value;
                    }
                }
            }
        }
        #endregion
    }
}
