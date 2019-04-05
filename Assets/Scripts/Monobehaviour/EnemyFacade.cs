using UnityEngine;
using Zenject;

public class EnemyFacade : MonoBehaviour
{
    public class Pool : MonoMemoryPool<Vector3, EnemyFacade>
    {
        protected override void Reinitialize(Vector3 position, EnemyFacade item)
        {
            item.transform.position = position;
        }
    }
}
