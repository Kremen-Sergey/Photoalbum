using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using DAL.Interface.Repository;

namespace BLL.Services
{
    public class AlbumService: IAlbumService
    {
        private readonly IUnitOfWork uow ;
        private readonly IAlbumRepository albumRepository;

        public AlbumService(IUnitOfWork uow, IAlbumRepository albumRepository)
        {
            this.uow = uow;
            this.albumRepository = albumRepository;
            Debug.WriteLine("Album service create!");
        }

        public IEnumerable<AlbumEntity> GetAll()
        {
            return albumRepository.GetAll().Select(album => album.ToBllAlbum());
        }

        public void Create(AlbumEntity album)
        {
            albumRepository.Create(album.ToDalAlbum());
            uow.Commit();
        }

        public void Delete(AlbumEntity album)
        {
            albumRepository.Delete(album.ToDalAlbum());
            uow.Commit();
        }

        public AlbumEntity GetById(int id)
        {
            if (albumRepository.GetById(id) == null) { return null; }
            return albumRepository.GetById(id).ToBllAlbum();
        }
    }
}

