using Wolf_Island.Core.GameSnapshots;

namespace Wolf_Island.App.View
{
    internal interface IRender
    {
        void Render(ObjectStyleGameSnapshot snapshot);
    }
}