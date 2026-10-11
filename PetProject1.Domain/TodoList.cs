using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace PetProject1.Domain
{
    public class TodoList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }
        public string OwnerId { get; set; }
        public DateTime CreatedAt {  get; set; }

        public List<string> SharedUserIds { get; set; } = new();
    }
}
