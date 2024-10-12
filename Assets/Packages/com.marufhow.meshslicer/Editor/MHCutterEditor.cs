using com.marufhow.meshslicer.core;

namespace Packages.com.marufhow.meshslicer.Editor
{
   
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(MHCutter))]
    public class MHCutterEditor : Editor
    {
        private VisualTreeAsset _visualTree;

        private void OnEnable()
        {
            _visualTree = Resources.Load<VisualTreeAsset>("MHCutterInspector");
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            if (_visualTree != null)
            {
                _visualTree.CloneTree(root);
            }
            else
            {
                Debug.LogError("MHCutterInspector UXML not found in Resources.");
            }
            return root;
        }
    }


}