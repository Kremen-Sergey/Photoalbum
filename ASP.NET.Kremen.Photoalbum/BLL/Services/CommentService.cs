using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using DAL.Interface.Repository;

namespace BLL.Services
{
    public class CommentService: ICommentService
    {
        private readonly IUnitOfWork uow;
        private readonly ICommentRepository commentRepository;

        public CommentService(IUnitOfWork uow, ICommentRepository repository)
        {
            this.uow = uow;
            this.commentRepository = repository;
            Debug.WriteLine("Comment service create!");
        }
        public IEnumerable<CommentEntity> GetAll()
        {
            return commentRepository.GetAll().Select(comment => comment.ToBllComment());
        }

        public void Create(CommentEntity comment)
        {
            commentRepository.Create(comment.ToDalComment());
            uow.Commit();
        }

        public void Delete(CommentEntity comment)
        {
            commentRepository.Delete(comment.ToDalComment());
            uow.Commit();
        }

        public CommentEntity GetById(int id)
        {
            if (commentRepository.GetById(id) == null) { return null; }
            return commentRepository.GetById(id).ToBllComment();
        }
    }
}
