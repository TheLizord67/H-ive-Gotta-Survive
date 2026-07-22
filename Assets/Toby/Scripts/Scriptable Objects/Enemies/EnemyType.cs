using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable Objects/EnemyType")]
public partial class EnemyType : ScriptableObject
{
    [SerializeField] public EnemyType_ enemyType;
}
