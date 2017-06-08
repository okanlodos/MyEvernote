using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();

        public Test()
        {
            List<Category> categories = repo_category.List();
        }

        public void InsertTest()
        {
            int result = repo_user.Insert(new EvernoteUser()
            {
                Name = "InsertTest",
                Surname = "InsertTest",
                Email = "InsertTest@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                isActive = true,
                IsAdmin = false,
                Username = "InsertTest",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "okanlodos"
            });
        }

        public void UpdateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "InsertTest");
            if (user != null)
            {
                user.Username = "xxx";
                int result = repo_user.Update(user);
            }
        }

        public void DeleteTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "xxx");
            if (user != null)
            {
                int result = repo_user.Delete(user);
            }
        }

        public void CommentTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Id == 1);
            Note note = repo_note.Find(x => x.Id == 3);
            Comment comment = new Comment()
            {
                Text = "Bu Bir Testtir.22",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "okanlodos",
                Note = note,
                Owner = user
            };
            repo_comment.Insert(comment);
        }
    }
}
