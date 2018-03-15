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
            if (listBox.SelectedItem != null)
            {
                switch (listBox.SelectedIndex)
                {
                    case 0:
                        example = new VertexProgram();
                        break;
                    case 1:
                        example = new FragmentProgram();
                        break;
                    case 2:
                        example = new UniformParameter();
                        break;
                    case 3:
                        example = new VaryingParameter();
                        break;
                    case 4:
                        example = new TextureSampling();
                        break;
                    case 5:
                        example = new VertexTwisting();
                        break;
                    case 6:
                        example = new TwoTextureAccesses();
                        break;
                    case 7:
                        example = new VertexTransform();
                        break;
                    case 8:
                        example = new VertexLighting();
                        break;
                    case 9:
                        example = new FragmentLighting();
                        break;
                }
            }
        }

        private void BtnRunClick(object sender, EventArgs e)
        {
            example?.Start();
        }
    }
}