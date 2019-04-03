using Unity.Entities;

namespace Components
{
    public struct InputComponent : IComponentData
    {
        public float Horizontal;
        public float Vertical;
    }
}