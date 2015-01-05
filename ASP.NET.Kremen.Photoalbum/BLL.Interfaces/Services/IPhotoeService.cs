using BLL.Interfaces.Entities;

namespace BLL.Interfaces.Services
{
    public interface IPhotoeService : IService<PhotoeEntity>
    {
        void Update(PhotoeEntity photo);
    }
}
