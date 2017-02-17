using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf.SharpDX;
using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using OrthographicCamera = HelixToolkit.Wpf.SharpDX.OrthographicCamera;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;
// ReSharper disable All

namespace HDEarth
{
    /// <summary>
    ///     Base ViewModel for Demo Applications?
    /// </summary>
    public abstract class BaseViewModel : ObservableObject
    {
        public const string Orthographic = "Orthographic Camera";

        public const string Perspective = "Perspective Camera";

        private Camera camera;

        private string cameraModel;

        protected OrthographicCamera defaultOrthographicCamera = new OrthographicCamera
        {
            Position = new Point3D(0, 0, 5),
            LookDirection = new Vector3D(-0, -0, -5),
            UpDirection = new Vector3D(0, 1, 0),
            NearPlaneDistance = 1,
            FarPlaneDistance = 100
        };

        protected PerspectiveCamera defaultPerspectiveCamera = new PerspectiveCamera
        {
            Position = new Point3D(0, 0, 5),
            LookDirection = new Vector3D(-0, -0, -5),
            UpDirection = new Vector3D(0, 1, 0),
            NearPlaneDistance = 0.5,
            FarPlaneDistance = 1500000
        };

        private RenderTechnique renderTechnique;

        private string subTitle;

        private string title;

        protected BaseViewModel()
        {
            // camera models
            CameraModelCollection = new List<string>
            {
                Orthographic,
                Perspective
            };

            // on camera changed callback
            CameraModelChanged += (s, e) =>
            {
                if (cameraModel == Orthographic)
                {
                    Camera = defaultOrthographicCamera;
                }
                else if (cameraModel == Perspective)
                {
                    Camera = defaultPerspectiveCamera;
                }
                else
                    throw new HelixToolkitException("Camera Model Error.");
            };

            // default camera model
            CameraModel = Perspective;

            Title = "Demo (HelixToolkitDX)";
            SubTitle = "Default Base View Model";
        }

        public string Title
        {
            get { return title; }
            set { SetValue(ref title, value, "Title"); }
        }

        public string SubTitle
        {
            get { return subTitle; }
            set { SetValue(ref subTitle, value, "SubTitle"); }
        }

        public RenderTechnique RenderTechnique
        {
            get { return renderTechnique; }
            set { SetValue(ref renderTechnique, value, "RenderTechnique"); }
        }

        public List<string> ShadingModelCollection { get; private set; }

        public List<string> CameraModelCollection { get; private set; }

        public string CameraModel
        {
            get { return cameraModel; }
            set
            {
                if (SetValue(ref cameraModel, value, "CameraModel"))
                    OnCameraModelChanged();
            }
        }

        public Camera Camera
        {
            get { return camera; }

            protected set
            {
                SetValue(ref camera, value, "Camera");
                CameraModel = value is PerspectiveCamera
                    ? Perspective
                    : value is OrthographicCamera ? Orthographic : null;
            }
        }

        public IEffectsManager EffectsManager { get; protected set; }

        public IRenderTechniquesManager RenderTechniquesManager { get; protected set; }

        public event EventHandler CameraModelChanged;

        protected virtual void OnCameraModelChanged()
        {
            var eh = CameraModelChanged;
            if (eh != null)
                eh(this, new EventArgs());
        }
    }


    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string info = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        protected bool SetValue<T>(ref T backingField, T value, string propertyName)
        {
            if (Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}