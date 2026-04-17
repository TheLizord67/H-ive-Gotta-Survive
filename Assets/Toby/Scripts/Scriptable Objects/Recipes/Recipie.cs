using System.Collections.Generic;

public partial class Recipies
{
    [System.Serializable]
    public struct Recipie
    {
        public string name;

        public List<_Resources> resources;

        public Item result;
    }
}
