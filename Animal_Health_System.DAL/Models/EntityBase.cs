using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Models
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get;  set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get;  set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }

        public void SetCreatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }
        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
