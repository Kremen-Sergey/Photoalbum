using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using DAL.Interface.Repository;

namespace BLL.Services
{
    public class PhotoeService: IPhotoeService
    {
        private readonly IUnitOfWork uow;
        private readonly IPhotoeRepository photoeRepository;
        public PhotoeService(IUnitOfWork uow, IPhotoeRepository photoeRepository)
        {
            this.uow = uow;
            this.photoeRepository = photoeRepository;
            Debug.WriteLine("Photo service create!");
        }

        public IEnumerable<PhotoeEntity> GetAll()
        {
            return photoeRepository.GetAll().Select(photoe => photoe.ToBllPhotoe());
        }

        public void Create(PhotoeEntity photoe)
        {
            photoeRepository.Create(photoe.ToDalPhotoe());
            uow.Commit();
        }

        public void Delete(PhotoeEntity photoe)
        {
            photoeRepository.Delete(photoe.ToDalPhotoe());
            uow.Commit();
        }

        public PhotoeEntity GetById(int id)
        {
            return photoeRepository.GetById(id).ToBllPhotoe();
        }

        public void Update(PhotoeEntity photo)
        {
            photoeRepository.Update(photo.ToDalPhotoe());
            uow.Commit();
        }
    }
}
