using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendMessage
{
    public class HttpBody
    {
        [Required]
        [Range(1000, 9999)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [Range(100000000, 9999999999)]
        public long BusinessPhone { get; set; }

        [Required]
        [Range(100000000, 9999999999)]
        public long CustomerPhone { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Message {  get; set; }
    }
}
