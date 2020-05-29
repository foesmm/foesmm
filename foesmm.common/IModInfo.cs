using System;

namespace foesmm
{
    public interface IModInfo
    {
        Guid Guid { get; }
        string Title { get; }
        string Description { get; }
    }
}