using CoderSight.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoderSight.Data;

public class CmsDbContext : IdentityDbContext<ApplicationUser>
{
    public CmsDbContext(DbContextOptions<CmsDbContext> options) : base(options) { }

    public DbSet<Page> Pages => Set<Page>();
    public DbSet<PageBlock> PageBlocks => Set<PageBlock>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<BlogPostCategory> BlogPostCategories => Set<BlogPostCategory>();
    public DbSet<BlogPostTag> BlogPostTags => Set<BlogPostTag>();
    public DbSet<Media> Media => Set<Media>();
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();
    public DbSet<NavMenuItem> NavMenuItems => Set<NavMenuItem>();
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<BlogComment> BlogComments => Set<BlogComment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Page>(e =>
        {
            e.HasIndex(p => new { p.Slug, p.Culture }).IsUnique();
            e.HasIndex(p => p.LocalizationGroupId);
            e.HasIndex(p => p.Status);
            e.Property(p => p.Status).HasConversion<string>();
        });

        builder.Entity<PageBlock>(e =>
        {
            e.HasIndex(pb => new { pb.PageId, pb.SortOrder });
            e.Property(pb => pb.Data).HasColumnType("nvarchar(max)");
            e.Property(pb => pb.Styles).HasColumnType("nvarchar(max)").HasDefaultValue("{}");
            e.HasOne(pb => pb.Page)
                .WithMany(p => p.Blocks)
                .HasForeignKey(pb => pb.PageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<BlogPost>(e =>
        {
            e.HasOne(bp => bp.Page)
                .WithOne(p => p.BlogPost)
                .HasForeignKey<BlogPost>(bp => bp.PageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Category>(e =>
        {
            e.HasIndex(c => new { c.Slug, c.Culture }).IsUnique();
            e.HasIndex(c => c.LocalizationGroupId);
        });

        builder.Entity<Tag>(e =>
        {
            e.HasIndex(t => new { t.Slug, t.Culture }).IsUnique();
            e.HasIndex(t => t.LocalizationGroupId);
        });

        builder.Entity<BlogPostCategory>(e =>
        {
            e.HasKey(bc => new { bc.BlogPostId, bc.CategoryId });
            e.HasOne(bc => bc.BlogPost).WithMany(bp => bp.BlogPostCategories).HasForeignKey(bc => bc.BlogPostId);
            e.HasOne(bc => bc.Category).WithMany(c => c.BlogPostCategories).HasForeignKey(bc => bc.CategoryId);
        });

        builder.Entity<BlogPostTag>(e =>
        {
            e.HasKey(bt => new { bt.BlogPostId, bt.TagId });
            e.HasOne(bt => bt.BlogPost).WithMany(bp => bp.BlogPostTags).HasForeignKey(bt => bt.BlogPostId);
            e.HasOne(bt => bt.Tag).WithMany(t => t.BlogPostTags).HasForeignKey(bt => bt.TagId);
        });

        builder.Entity<NavMenuItem>(e =>
        {
            e.HasOne(m => m.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(m => new { m.ParentId, m.SortOrder });
        });

        builder.Entity<Subscriber>(e =>
        {
            e.HasIndex(s => s.Email).IsUnique();
            e.HasIndex(s => s.UnsubscribeToken).IsUnique();
        });

        builder.Entity<BlogComment>(e =>
        {
            e.HasOne(c => c.BlogPost)
                .WithMany()
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(c => new { c.BlogPostId, c.IsApproved, c.CreatedAt });
        });

        builder.Entity<SiteSettings>(e =>
        {
            e.HasData(new SiteSettings
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                SiteName = "CoderSight",
                DefaultCulture = "en",
                SupportedCultures = "en,fa",
                DefaultMetaDescription = "CoderSight — Your platform for developer insights",
                FooterText = "© 2026 CoderSight. All rights reserved."
            });
        });
    }
}
