using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EqDemo.Models
{

    public class Category
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("CategoryID")]
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public byte[] Picture { get; set; }

    }
}