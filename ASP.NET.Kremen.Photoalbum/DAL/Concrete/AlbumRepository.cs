using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using DAL.Interface.DTO;
using DAL.Interface.Repository;
using ORM;

namespace DAL.Concrete
{
    public class AlbumRepository : IAlbumRepository
    {
         private readonly DbContext context;

         public AlbumRepository(IUnitOfWork uow)
        {
            this.context = uow.Context;
            Debug.WriteLine("AlbumRepository create!");
        }

        public IEnumerable<DalAlbum> GetAll()
        {
            return context.Set<Album>().Select(album => new DalAlbum()
            {
                Id = album.Id,
                AlbumName = album.AlbumName,
                Description = album.Description,
                UserId = album.UserId,
                CreationTime = album.CreationTime
            });
        }

        public DalAlbum GetById(int key)
        {
            var ormalbum = context.Set<Album>().Find(key);
            return new DalAlbum()
            {
                Id = ormalbum.Id,
                AlbumName = ormalbum.AlbumName,
                Description = ormalbum.Description,
                UserId = ormalbum.UserId,
                CreationTime = ormalbum.CreationTime
            };
        }

        public DalAlbum GetByPredicate(System.Linq.Expressions.Expression<Func<DalAlbum, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Create(DalAlbum e)
        {
            var album = new Album()
            {
                Id = e.Id,
                AlbumName = e.AlbumName,
                Description = e.Description,
                UserId = e.UserId,
                CreationTime = e.CreationTime
            };
            context.Set<Album>().Add(album);
        }

        public void Delete(DalAlbum e)
        {
            var ormalbum = context.Set<Album>().FirstOrDefault(album => album.Id == e.Id);
            if (ormalbum != null)
            {
                context.Set<Album>().Remove(ormalbum);
            }
        }

        public void Update(DalAlbum entity)
        {
            throw new NotImplementedException();
        }
    }
}
