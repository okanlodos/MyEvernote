using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Okan",
                Surname = "Lodos",
                Email = "okanlodos@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                isActive = true,
                IsAdmin = true,
                Username = "okanlodos",
                Password = "123456",
                ProfilImagePath = "user.png",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "okanlodos"
            };

            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Okan2",
                Surname = "Lodos2",
                Email = "okanlodos2@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                isActive = true,
                IsAdmin = false,
                Username = "okanlodos2",
                ProfilImagePath = "user.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "okanlodos"
                
            };
            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            for (int b = 0; b < 8; b++)
            {
                EvernoteUser us = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    isActive = true,
                    IsAdmin = false,
                    Username = $"user{b}",
                    Password = "123",
                    ProfilImagePath = "user.png",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{b}"
                };
                context.EvernoteUsers.Add(us);
            }

            context.SaveChanges();
            List<EvernoteUser> es = context.EvernoteUsers.ToList();

            // Adding fake category
            for (int i = 0; i < 10; i++)
            {
                Category category = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    ModifiedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    ModifiedUsername = "okanlodos"
                };

                context.Categories.Add(category);

                // Adding fake notes
                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    EvernoteUser ev_user = es[FakeData.NumberData.GetNumber(0, es.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        //Category = category,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 8),
                        Owner = ev_user,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = ev_user.Username
                    };

                    category.Notes.Add(note);

                    // Adding fake comments
                    for (int s = 0; s < FakeData.NumberData.GetNumber(1, 5); s++)
                    {
                        EvernoteUser ev_user2 = es[FakeData.NumberData.GetNumber(0, es.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            //Note = note,
                            Owner = ev_user2,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = ev_user2.Username
                        };

                        note.Comments.Add(comment);
                        for (int m = 0; m < note.LikeCount; m++)
                        {
                            Liked liked = new Liked()
                            {
                                LikedUser = es[m],

                            };
                            note.Likes.Add(liked);
                        }
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
