namespace Signals
{
    public struct SignalUiLayerWantsAddEnemy
    {
        public int Count { get; }

        public SignalUiLayerWantsAddEnemy(int count)
        {
            Count = count;
        }
    }
}