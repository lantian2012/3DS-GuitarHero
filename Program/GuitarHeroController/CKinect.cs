using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Kinect;


namespace GuitarHeroController
{
    class CKinect
    {
        private KinectSensor _kinect;
        private int _x;
        private int _y;
        public CKinect()
        {
            initializeKinect();
        }
        private void initializeKinect()
        {

            foreach (var potentialKinect in KinectSensor.KinectSensors)
            {
                if (potentialKinect.Status == KinectStatus.Connected)
                {
                    this._kinect = potentialKinect;
                    break;
                }
            }
            if (this._kinect != null)
            {
                TransformSmoothParameters parameters = new TransformSmoothParameters();
                parameters.Smoothing = (float)0.75;
                parameters.Prediction = 0;
                parameters.JitterRadius = (float)0.05;
                parameters.MaxDeviationRadius = (float)(0.04);
                this._kinect.SkeletonStream.Enable(parameters);
            }
        }
        public Boolean start()
        {
            try
            {
                this._kinect.Start();
            }
            catch (IOException)
            {
                this._kinect = null;
            }
            if (this._kinect != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady
        {
            set { this._kinect.SkeletonFrameReady += value; }
        }
        public int getX()
        {
            return 0;
        }
        public int ElevationAngle
        {
            get { return this._kinect.ElevationAngle; }
            set
            {
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
}
