namespace Signals
{
    public struct SignalEcsLayerPlayerCountUpdate
    {
        public int Count { get; }

        public SignalEcsLayerPlayerCountUpdate(int count)
        {
            Count = count;
        }
    }
}