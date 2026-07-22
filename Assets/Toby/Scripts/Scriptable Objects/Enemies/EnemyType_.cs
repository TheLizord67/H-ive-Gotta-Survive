using UnityEngine;

public partial class EnemyType
{
    [System.Serializable]
    public struct EnemyType_
    {
        public string name;

        public int retreatHealthCurrent;

        public int retreatHealthMax;

        public float speed;

        public int damage;

        public float attackSpeed;

        public GameObject currentTarget;

        public float searchDistance;

        public float hitDistance;

        public float retreatDistance;

        public float followTimeMax;

        public float followTime;

        public float rotateSpeed;
    }
}
