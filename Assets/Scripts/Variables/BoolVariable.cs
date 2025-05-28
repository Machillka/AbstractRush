using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "Variables/BoolVariable")]
public class BoolVariable : ScriptableObject
{
    [TextArea]
    public string description;

    public bool value { get; set; }
}
