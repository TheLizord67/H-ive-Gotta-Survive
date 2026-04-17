using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipies", menuName = "Scriptable Objects/Recipies")]
public partial class Recipies : ScriptableObject
{
    public List<Recipie> recipies;
}
