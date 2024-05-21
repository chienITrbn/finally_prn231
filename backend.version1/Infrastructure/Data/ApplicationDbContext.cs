using backend.version1.Domain.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(p => p.Title).HasColumnType("varchar(255)").IsRequired();
                entity.Property(p => p.Content).HasColumnType("text").IsRequired();
                entity.Property(p => p.Image).HasColumnType("text");
                entity.Property(p => p.Tags).HasColumnType("text");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Posts_Users");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(c => c.Content).HasColumnType("text").IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Comments_Users");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_Comments_Posts");
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.CommentId });

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PostComments_Posts");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PostComments_Comments");
            });

            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Reactions_Users");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Reactions_Posts");
            });

            modelBuilder.Entity<PostReaction>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.ReactionId });

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostReactions)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PostReactions_Posts");

                entity.HasOne(d => d.Reaction)
                .WithMany(p => p.PostReactions)
                    .HasForeignKey(d => d.ReactionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PostReactions_Reactions");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.RefreshToken).HasColumnType("text");

                entity.HasMany(u => u.Posts)
                    .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict) // or DeleteBehavior.SetNull depending on your requirements
                    .HasConstraintName("FK_Posts_Users");

                // Disable cascade behavior for the Comments relationship
                entity.HasMany(u => u.Comments)
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict) // or DeleteBehavior.SetNull depending on your requirements
                    .HasConstraintName("FK_Comments_Users");

                entity.HasMany(u => u.Reactions)
                    .WithOne(r => r.User)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Reactions_Users");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Name).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(p => p.Description).HasColumnType("nvarchar(500)").IsRequired();
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name).HasColumnType("nvarchar(100)").IsRequired();
            });

            //modelBuilder.Entity<Friendship>(entity =>
            //{
            //    entity.HasKey(f => new { f.UserId, f.FriendId });

            //    entity.HasOne(f => f.User)
            //        .WithMany(u => u.Friendships)
            //        .HasForeignKey(f => f.UserId)
            //        .HasConstraintName("FK_Friendships_Users");

            //    entity.HasOne(f => f.Friend)
            //        .WithMany()
            //        .HasForeignKey(f => f.FriendId)
            //        .HasConstraintName("FK_Friendships_Friends");
            //});
        }
    }
}