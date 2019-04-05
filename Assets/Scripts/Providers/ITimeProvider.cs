namespace Providers
{
    public interface ITimeProvider
    {
        float DeltaTime { get; }
        void SetDeltaTime(float dt);
    }
}