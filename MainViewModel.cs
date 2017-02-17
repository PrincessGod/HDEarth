using System.Windows.Media;

namespace HDEarth
{
    using System.Linq;
    using HelixToolkit.Wpf.SharpDX;
    using HelixToolkit.Wpf.SharpDX.Core;

    using SharpDX;
    using Media3D = System.Windows.Media.Media3D;
    using Point3D = System.Windows.Media.Media3D.Point3D;
    using Vector3D = System.Windows.Media.Media3D.Vector3D;
    using HelixToolkit.Wpf;
    using System.Windows.Media.Imaging;
    using System.IO;
    public class MainViewModel:BaseViewModel
    {
        public MeshGeometry3D Model { get; private set; }

        public PhongMaterial RedMaterial { get; private set; }

        public Vector3 DirectionalLightDirection { get; private set; }
        public Color4 DirectionalLightColor { get; private set; }
        public Color4 AmbientLightColor { get; private set; }

        public MainViewModel()
        {
            // titles
            Title = "Simple Demo";
            SubTitle = "WPF & SharpDX";

            // camera setup
            Camera = new PerspectiveCamera
            {
                Position = new Point3D(3, 3, 5),
                LookDirection = new Vector3D(-3, -3, -5),
                UpDirection = new Vector3D(0, 1, 0),
                FarPlaneDistance = 5000000
            };

            // default render technique
            RenderTechniquesManager = new DefaultRenderTechniquesManager();
            RenderTechnique = RenderTechniquesManager.RenderTechniques[DefaultRenderTechniqueNames.Blinn];
            EffectsManager = new DefaultEffectsManager(RenderTechniquesManager);

            // setup lighting            
            AmbientLightColor = new Color4(1f, 1f, 1f, 1f);
            DirectionalLightColor = Color.White;
            DirectionalLightDirection = new Vector3(-2, -5, -2);

            // scene model3d
            var b1 = new MeshBuilder();
            b1.AddSphere(new Vector3(0, 0, 0), 100000, 180, 90);

            var meshGeometry = b1.ToMeshGeometry3D();
            meshGeometry.Colors = new Color4Collection(meshGeometry.TextureCoordinates.Select(x => x.ToColor4()));
            Model = meshGeometry;

            // model materials
            var uri = new System.Uri(@"Earth4k.jpg", System.UriKind.RelativeOrAbsolute);
            var image = new FileStream(uri.ToString(), FileMode.Open);
            RedMaterial = new PhongMaterial
            {
                DiffuseColor = Color.White,
                AmbientColor = Color.Black,
                ReflectiveColor = Color.Black,
                EmissiveColor = Color.Black,
                SpecularColor = Color.Black,
                DiffuseAlphaMap = image,
            };
        }
    }
}
