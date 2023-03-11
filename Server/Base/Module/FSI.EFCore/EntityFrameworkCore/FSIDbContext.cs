using FSI.Domain.Chat;
using FSI.Domain.File;
using FSI.Domain.Test;
using FSI.Domain.User;
using FSI.Domain.Founder;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EntityFrameworkCore;

[ConnectionStringName("DefaultConnection")]
public class FSIDbContext :
    AbpDbContext<FSIDbContext>
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //AuditLogging

    #endregion

    public DbSet<Test> Tests { get; set; }
    public DbSet<UserRoot> UserRoots { get; set; }
    public DbSet<Founder> Founders { get; set; }
    public DbSet<FileInfomation> Files { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<UserConversation> UserConversations { get; set; }
    public DbSet<UserConnection> UserConnections { get; set; }


    public FSIDbContext(DbContextOptions<FSIDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Model.SetMaxIdentifierLength(30);
        /* Include modules to your migration db context */


        /* Configure your own tables/entities inside here */
        builder.Entity<Test>(entity =>
        {
            entity.HasKey(x => x.Id).HasName("SYS_C123");
            entity.ToTable("FSI_TEST");
            entity.Property(x => x.Code).HasColumnName("CODE");
            entity.Property(x => x.Name).HasColumnName("NAME");
            entity.Property(x => x.Description).HasColumnName("DESCRIPTION");
        });
    }
}
