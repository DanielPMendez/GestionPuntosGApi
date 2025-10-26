using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Model
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

    }
}
