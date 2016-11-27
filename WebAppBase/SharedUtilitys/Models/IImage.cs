using Pasys.Core.IOC;

namespace Pasys.Core.Models
{
    public interface IImage : IEntity
    {
        string ImageUrl { get; set; }
        string ImageThumbUrl { get; set; }
    }
}
