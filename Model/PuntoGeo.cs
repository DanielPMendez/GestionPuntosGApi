using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PuntoGeo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("latitud")]
        public double Latitud { get; set; }

        [BsonElement("longitud")]
        public double Longitud { get; set; }

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
