using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HolesManager))]
public class HolesManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        var mng = target as HolesManager;

        if (mng == null)
            return;

        foreach(var hole in mng.holes)
        {
            hole.position = Handles.PositionHandle(hole.position, Quaternion.identity);
        }
    }
}
