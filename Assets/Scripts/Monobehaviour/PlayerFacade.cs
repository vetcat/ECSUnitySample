using UnityEngine;
using Zenject;

public class PlayerFacade : MonoBehaviour
{
    public class Pool : MonoMemoryPool<Vector3, PlayerFacade>
    {
        protected override void Reinitialize(Vector3 position, PlayerFacade item)
        {
            item.transform.position = position;
        }
    }
}
