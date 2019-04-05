namespace Providers
{
    public class TimeProvider : ITimeProvider
    {
        public float DeltaTime => _deltaTime;
        private float _deltaTime;

        public void SetDeltaTime(float dt)
        {
            _deltaTime = dt;
        }
    }
}
