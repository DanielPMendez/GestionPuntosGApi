using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class PuntoGeo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "La latitud es requerida.")]
        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
        [BsonElement("latitud")]
        public double Latitud { get; set; }

        [Required(ErrorMessage = "La longitud es requerida.")]
        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
        [BsonElement("longitud")]
        public double Longitud { get; set; }

        [Required(ErrorMessage = "El tipo es requerido.")]
        [RegularExpression("accidente|congestión|obstrucción|otro", ErrorMessage = "Tipo inválido.")]
        [BsonElement("tipo")]
        public string Tipo { get; set; }

        [BsonElement("descripcion")]
        public string? Descripcion { get; set; }

        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [BsonElement("usuarioIngreso")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioIngreso { get; set; }

    }
}
