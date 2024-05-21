using backend.version1.Domain.Entities;
using Bogus;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace backend.version1.Infrastructure.Seeders
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _context;

        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var categoryIds = new List<int>();

                if (!_context.Categories.Any())
                {
                    var categoryFaker = new Faker<Category>()
                        .RuleFor(c => c.Name, f => f.Commerce.Department());

                    var categories = categoryFaker.Generate(10);

                    _context.Categories.AddRange(categories);
                    _context.SaveChanges();

                    categoryIds = categories.Select(c => c.Id).ToList();
                }

                if (!_context.Products.Any())
                {
                    var productFaker = new Faker<Product>()
                        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                        .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                        .RuleFor(p => p.Price, f => f.Random.Decimal(1.0m, 100.0m))
                        .RuleFor(p => p.CategoryId, (f, p) => f.PickRandom(categoryIds));

                    var products = productFaker.Generate(50);

                    _context.Products.AddRange(products);
                    _context.SaveChanges();
                }

                if (!_context.Roles.Any())
                {
                    var roles = new List<IdentityRole>
                {
                    new IdentityRole { Name = "SuperAdmin", NormalizedName = "SA", ConcurrencyStamp = "1" },
                    new IdentityRole { Name = "Sale", NormalizedName = "SALE", ConcurrencyStamp = "2" },
                    new IdentityRole { Name = "Guest", NormalizedName = "GU", ConcurrencyStamp = "3" }
                };

                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }

                if (!_context.Users.Any())
                {
                    var userFaker = new Faker<User>()
                        .RuleFor(u => u.UserName, f => f.Internet.UserName())
                        .RuleFor(u => u.Email, f => f.Internet.Email())
                        .RuleFor(u => u.PasswordHash, f => f.Internet.Password());

                    var users = userFaker.Generate(10);

                    _context.Users.AddRange(users);
                    _context.SaveChanges();
                }

                if (!_context.Posts.Any())
                {
                    var postFaker = new Faker<Post>()
                        .RuleFor(p => p.Title, f => f.Lorem.Sentence())
                        .RuleFor(p => p.Content, f => f.Lorem.Paragraph())
                        .RuleFor(p => p.Created, f => f.Date.Past())
                        .RuleFor(p => p.UserId, f => f.PickRandom(_context.Users.Select(u => u.Id).ToList()))
                        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                        .RuleFor(p => p.Tags, f => f.Lorem.Word());

                    var posts = postFaker.Generate(50);
                    _context.Posts.AddRange(posts);
                    _context.SaveChanges();
                }

                if (!_context.Comments.Any())
                {
                    var commentFaker = new Faker<Comment>()
                        .RuleFor(c => c.Content, f => f.Lorem.Paragraph())
                        .RuleFor(c => c.Created, f => f.Date.Past())
                        .RuleFor(c => c.UserId, f => f.PickRandom(_context.Users.Select(u => u.Id).ToList()))
                        .RuleFor(c => c.PostId, f => f.PickRandom(_context.Posts.Select(p => p.Id).ToList()));

                    var comments = commentFaker.Generate(50);
                    _context.Comments.AddRange(comments);
                    _context.SaveChanges();
                }

                if (!_context.Reactions.Any())
                {
                    var reactionFaker = new Faker<Reaction>()
                        .RuleFor(r => r.UserId, f => f.PickRandom(_context.Users.Select(u => u.Id).ToList()))
                        .RuleFor(r => r.PostId, f => f.PickRandom(_context.Posts.Select(p => p.Id).ToList()))
                        .RuleFor(r => r.IsLike, f => f.Random.Bool());

                    var reactions = reactionFaker.Generate(50);
                    _context.Reactions.AddRange(reactions);
                    _context.SaveChanges();
                }

            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}