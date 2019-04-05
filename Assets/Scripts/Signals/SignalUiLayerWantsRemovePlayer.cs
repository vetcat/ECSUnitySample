namespace Signals
{
    public struct SignalUiLayerWantsRemovePlayer
    {
        public int Count { get; }

        public SignalUiLayerWantsRemovePlayer(int count)
        {
            Count = count;
        }
    }
}