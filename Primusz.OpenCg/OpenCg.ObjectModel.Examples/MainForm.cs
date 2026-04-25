using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace OpenCg.ObjectModel.Examples
{
    internal sealed class MainForm : Form
    {
        private readonly TreeView treeView;
        private readonly Button runButton;

        public MainForm()
        {
            Text = "OpenCg Object Model Examples";
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Width = 420;
            Height = 560;

            treeView = new TreeView
            {
                Left = 12,
                Top = 12,
                Width = 380,
                Height = 455,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom
            };
            treeView.NodeMouseDoubleClick += TreeViewNodeMouseDoubleClick;

            runButton = new Button
            {
                Left = 12,
                Top = 475,
                Width = 380,
                Height = 34,
                Text = "Run",
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            runButton.Click += RunButtonClick;

            Controls.Add(treeView);
            Controls.Add(runButton);

            LoadExamples();
            treeView.Sort();
            treeView.ExpandAll();
        }

        private void LoadExamples()
        {
            var examples = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(IExample).IsAssignableFrom(type) && !type.IsAbstract && type.GetConstructor(Type.EmptyTypes) != null)
                .Select(type => new
                {
                    Type = type,
                    Attribute = type.GetCustomAttributes(typeof(ExampleAttribute), false).Cast<ExampleAttribute>().FirstOrDefault()
                })
                .Where(example => example.Attribute != null)
                .OrderBy(example => example.Attribute.Path);

            foreach (var example in examples)
            {
                AddExample(example.Attribute.Path, example.Type.GetConstructor(Type.EmptyTypes));
            }
        }

        private void AddExample(string path, ConstructorInfo constructor)
        {
            TreeNodeCollection nodes = treeView.Nodes;
            string[] parts = path.Split('/');

            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (!nodes.ContainsKey(part))
                {
                    nodes.Add(part, part);
                }

                TreeNode node = nodes[part];
                if (i == parts.Length - 1)
                {
                    node.Tag = constructor;
                }

                nodes = node.Nodes;
            }
        }

        private void RunSelectedExample()
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Tag == null)
            {
                return;
            }

            using (var example = (IExample)((ConstructorInfo)treeView.SelectedNode.Tag).Invoke(null))
            {
                example.Start();
            }
        }

        private void RunButtonClick(object sender, EventArgs e)
        {
            RunSelectedExample();
        }

        private void TreeViewNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            RunSelectedExample();
        }
    }
}
