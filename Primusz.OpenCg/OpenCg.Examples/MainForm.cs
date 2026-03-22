using System;
using System.Windows.Forms;
using OpenCg.Examples.OpenTK.Basic;

namespace OpenCg.Examples
{
    public partial class MainForm : Form
    {
        private IExample example;

        public MainForm()
        {
            InitializeComponent();
        }

        private void ListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void BtnRunClick(object sender, EventArgs e)
        {
            if (listBox.SelectedItem == null)
                return;

            switch (listBox.SelectedIndex)
            {
                case 0: example = new VertexProgram(); break;
                case 1: example = new FragmentProgram(); break;
                case 2: example = new UniformParameter(); break;
                case 3: example = new VaryingParameter(); break;
                case 4: example = new TextureSampling(); break;
                case 5: example = new VertexTwisting(); break;
                case 6: example = new TwoTextureAccesses(); break;
                case 7: example = new VertexTransform(); break;
                case 8: example = new VertexLighting(); break;
                case 9: example = new FragmentLighting(); break;
                case 10: example = new TwoLightsWithStructs(); break;
                case 11: example = new LightAttenuation(); break;
                case 12: example = new Spotlight(); break;
                case 13: example = new Bulge(); break;
                case 14: example = new Particle(); break;
                case 15: example = new BumpMapping(); break;
                case 16: example = new ProjectiveTexturing(); break;
                case 17: example = new CubeMapReflection(); break;
                case 18: example = new CubeMapRefraction(); break;
                case 19: example = new ChromaticDispersion(); break;
                case 20: example = new SpecularBumpMap(); break;
                case 21: example = new BumpMapFloor(); break;
                case 22: example = new BumpMapTorus(); break;
                case 23: example = new UniformFog(); break;
                case 24: example = new ToonShading(); break;
            }

            example?.Start();
        }
    }
}
