using UnityEngine;

namespace Providers
{
    public class InputProvider : IInputProvider
    {
        public float Horizontal => Input.GetAxis("Horizontal");
        public float Vertical => Input.GetAxis("Vertical");
    }
}