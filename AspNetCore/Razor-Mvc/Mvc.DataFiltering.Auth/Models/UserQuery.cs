using Microsoft.AspNetCore.Identity;

namespace EqDemo.Models
{
    public class UserQuery
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ModelId { get; set; }

        public string QueryJson { get; set; }

        public string OwnerId { get; set; }

        public IdentityUser Owner { get; set; }
    }
}
