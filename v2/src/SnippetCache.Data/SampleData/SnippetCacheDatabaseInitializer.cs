using System.Collections.ObjectModel;
using SnippetCache.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetCache.Data.SampleData
{
    public class SnippetCacheDatabaseInitializer : 
        DropCreateDatabaseIfModelChanges<SnippetCacheDbContext>
// todo: For production, change to CreateDatabaseIfNotExists
    {
        private const int snippetCount = 1000;

        protected override void Seed(SnippetCacheDbContext context)
        {
            var languages = AddLanguages(context);
            var ratings = AddRatings(context);
            var files = AddFiles(context, 20);
            var hyperlinks = AddHyperlinks(context, 30);
            var comments = AddComments(context, 50);
            var users = AddUsers(context, 10);

            GenerateSnippets(500, languages, ratings, files, hyperlinks, users, comments, context);

        }

        private IList<Comment> AddComments(SnippetCacheDbContext context, int count)
        {
            var fileTextGenerator = new SampleTextGenerator();
            const SampleTextGenerator.SourceNames sourceNames = SampleTextGenerator.SourceNames.ChildHarold;

            var comments = new List<Comment>();
            for (int i = 0; i < count; i++)
            {
                var item = new Comment()
                {
                    Text = fileTextGenerator.GenSentences(2)
                };
                comments.Add(item);
                context.Comments.Add(item);
            }
            context.SaveChanges();
            return comments;
        }

        private void GenerateSnippets(int count, IList<Language> languages, IList<Rating> ratings, 
            IList<File> files, IList<Hyperlink> hyperlinks, IList<User> users, IList<Comment> comments, SnippetCacheDbContext context)
        {
            var rand = new Random();
            var bioTextGenerator = new SampleTextGenerator();
            const SampleTextGenerator.SourceNames bioTextSource = SampleTextGenerator.SourceNames.LoremIpsum;

            var snippets = new List<Snippet>();
            while (count-- > 0)
            {
                var item = new Snippet()
                    {
                        Description = bioTextGenerator.GenSentences(5),
                        PreviewData = bioTextGenerator.GenSentences(2),
                        IsFavorite = (rand.Next(1) % 1 == 0),
                        IsPublic = (rand.Next(1) % 1 == 0),
                        Language = languages[rand.Next(languages.Count)],
                        Comments = new Collection<Comment>(comments),
                        Files = new Collection<File>(files),
                        Hyperlinks = new Collection<Hyperlink>(hyperlinks)
                    };
            }
        }

        private IList<User> AddUsers(SnippetCacheDbContext context, int count)
        {
            var users = new List<User>();
            var enumerator = PeopleNames.RandomNameEnumerator(PeopleNames.NameGender.Both);
            var bioTextGenerator = new SampleTextGenerator();
            const SampleTextGenerator.SourceNames bioTextSource = SampleTextGenerator.SourceNames.TheRaven;

            while (count-- > 0)
            {
                enumerator.MoveNext();
                var name = enumerator.Current;

                var fullName = name.First + " " + name.Last;
                var item = new User()
                    {
                        FirstName = name.First,
                        LastName = name.Last,
                        Blog = String.Empty,
                        Email = bioTextGenerator.GenWords(1) + "@" + bioTextGenerator.GenWords(1) + ".com",
                        Twitter = "@" + bioTextGenerator.GenWords(1)
                    };
                users.Add(item);
                context.Users.Add(item);
            }
            context.SaveChanges();
            return users;
        }

        private IList<Hyperlink> AddHyperlinks(SnippetCacheDbContext context, int count)
        {
            var fileTextGenerator = new SampleTextGenerator();
            const SampleTextGenerator.SourceNames sourceNames = SampleTextGenerator.SourceNames.ChildHarold;

            var hyperlinks = new List<Hyperlink>();
            for (int i = 0; i < count; i++)
            {
                var item = new Hyperlink()
                {
                    Uri = fileTextGenerator.GenWords(1),
                    Description = fileTextGenerator.GenSentences(5)
                };
                hyperlinks.Add(item);
                context.Hyperlinks.Add(item);
            }
            context.SaveChanges();
            return hyperlinks;
        }

        private IList<File> AddFiles(SnippetCacheDbContext context, int count)
        {
            var fileTextGenerator = new SampleTextGenerator();
            const SampleTextGenerator.SourceNames sourceNames = SampleTextGenerator.SourceNames.ChildHarold;

            var files = new List<File>();
            for (int i = 0; i < count; i++)
            {
                var item = new File()
                    {
                        Name = fileTextGenerator.GenWords(1, sourceNames),
                        Description = fileTextGenerator.GenSentences(5),
                        Location =
                            @"C:\temp\" + fileTextGenerator.GenWords(1, SampleTextGenerator.SourceNames.TheRaven) +
                            ".txt"
                    };
                files.Add(item);
                context.Files.Add(item);
            }
            context.SaveChanges();
            return files;
        }

        private IList<Rating> AddRatings(SnippetCacheDbContext context)
        {
            var names = new[]
                {
                    "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"
                };

            var ratings = new List<Rating>();
            Array.ForEach(names, name =>
                {
                    var item = new Rating()
                        {
                            Value = name
                        };
                    ratings.Add(item);
                    context.Ratings.Add(item);
                });

            context.SaveChanges();
            return ratings;
        }

        private IList<Language> AddLanguages(SnippetCacheDbContext context)
        {
            var names = new[]
                {
                    "C#", "C", "VB", "VB.NET", "Python", "Scala", "Javascript", "TypeScript", "Objective C",
                    "F#", "T-SQL"
                };

            var languages = new List<Language>();
            Array.ForEach(names, name =>
                {
                    var item = new Language()
                        {
                            Name = name,
                            Version = "1.0"
                        };
                    languages.Add(item);
                    context.Languages.Add(item);
                });
            context.SaveChanges();
            return languages;
        }
    }
}
