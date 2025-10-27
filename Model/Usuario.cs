using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [BsonElement("username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

    }
}
