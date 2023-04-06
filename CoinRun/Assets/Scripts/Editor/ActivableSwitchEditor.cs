using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//custom editor pour verifier que l'objet renseigné implemente bien l'interface IActivable
[CustomEditor(typeof(ActivableSwitch))]
public class ActivableSwitchEditor : Editor
{

    ActivableSwitch _target;

    bool interfaceFound;

    bool objectAssigned;

    private void OnEnable()
    {
        _target = (ActivableSwitch)target;
        objectAssigned = _target.activableGO != null;
        if (objectAssigned) interfaceFound = _target.activableGO.GetComponent<IActivable>() != null;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!objectAssigned)
        {
            EditorGUILayout.HelpBox("No object assigned", MessageType.Error);
            return;
        }   
        if (!interfaceFound) EditorGUILayout.HelpBox("IActivable not found on object " + _target.activableGO.name, MessageType.Warning);
    }

}
