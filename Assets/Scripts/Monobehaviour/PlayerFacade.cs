using UnityEngine;
using Zenject;

public class PlayerFacade : MonoBehaviour
{
    private void Reset(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public class Pool : MonoMemoryPool<Vector3, Quaternion, PlayerFacade>
    {
        protected override void Reinitialize(Vector3 position, Quaternion rotation, PlayerFacade item)
        {
            item.Reset(position, rotation);
        }
    }
}
