using BakeryShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BakeryShop.Data.Configuration
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .Property(p => p.FirstName).IsRequired();
            builder
                .Property(p => p.LastName).IsRequired();
        }
    }
}
