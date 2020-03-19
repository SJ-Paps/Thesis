using UnityEditor;
using UnityEngine;

namespace SJ.Editor
{
    public static class SJUtilEditor
    {
        [MenuItem("GameObject/Create Organizational", priority = 0)]
        public static void CreateOrganizational()
        {
            GameObject go = new GameObject("-----------------------------");
            go.hideFlags = HideFlags.DontSaveInBuild;
        }
    }

}