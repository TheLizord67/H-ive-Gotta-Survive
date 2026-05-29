using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceGathering : MonoBehaviour
{
    [SerializeField] public List<_Resources> resource;
    [SerializeField] public float timeToGet;
    [SerializeField] public int lifeSpan;
}
