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
    public class CommentRepository : ICommentRepository
    {
        private readonly DbContext context;
        public CommentRepository(IUnitOfWork uow)
        {
            this.context = uow.Context;
            Debug.WriteLine("CommentRepository create!");
        }

        public IEnumerable<DallComment> GetAll()
        {
            return context.Set<Comment>().Select(comment => new DallComment()
                        {
                            Id = comment.Id,
                            UserId = comment.UserId,
                            PhotoId=comment.PhotoId,
                            TextComment=comment.TextComment,
                            CreateTime=comment.CreateTime
                        });
        }

        public DallComment GetById(int key)
        {
            var ormcomment = context.Set<Comment>().FirstOrDefault(comment => comment.Id == key);
            if (ormcomment == null) { return null; }
            return new DallComment()
            {
                Id = ormcomment.Id,
                UserId = ormcomment.UserId,
                PhotoId = ormcomment.PhotoId,
                TextComment = ormcomment.TextComment,
                CreateTime = ormcomment.CreateTime

            };
        }

        public DallComment GetByPredicate(System.Linq.Expressions.Expression<Func<DallComment, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Create(DallComment e)
        {
            var comment = new Comment()
            {
                Id = e.Id,
                UserId = e.UserId,
                PhotoId = e.PhotoId,
                TextComment = e.TextComment,
                CreateTime = e.CreateTime
            };
            context.Set<Comment>().Add(comment);
        }

        public void Delete(DallComment e)
        {
            var ormcomment = context.Set<Comment>().FirstOrDefault(comment => comment.Id == e.Id);
            if (ormcomment != null)
            {
                context.Set<Comment>().Remove(ormcomment);
            }
        }

        public void Update(DallComment entity)
        {
            throw new NotImplementedException();
        }
    }
}
