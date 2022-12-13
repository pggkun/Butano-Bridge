using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ButanoSettings", menuName = "Butano/Settings")]
public class ButanoSettings : ScriptableObject
{
    [SerializeField] private string projectIncludeFolder;
    [SerializeField] private string projectGFXFolder;
    public string ProjectIncludeFolder { get => projectIncludeFolder; set => projectIncludeFolder = value; }
    public string ProjectGFXFolder { get => projectGFXFolder; set => projectGFXFolder = value; }
}
