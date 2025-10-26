using Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL
{
    public class DalPuntoGeo : IDalPuntoGeo
    {
        private readonly IMongoCollection<PuntoGeo> _puntos;

        public DalPuntoGeo(IMongoDatabase database)
        {
            _puntos = database.GetCollection<PuntoGeo>("PuntoGeoreferencia");
        }

        public async Task<List<PuntoGeo>> ObtenerPuntoGeo(string? tipo, double? latitud, double? longitud, double? radioKm)
        {
            var filter = Builders<PuntoGeo>.Filter.Empty;

            if (!string.IsNullOrEmpty(tipo))
                filter &= Builders<PuntoGeo>.Filter.Eq(p => p.Tipo, tipo);

            if (latitud.HasValue && longitud.HasValue && radioKm.HasValue)
            {
                double latMin = latitud.Value - (radioKm.Value / 111.0);
                double latMax = latitud.Value + (radioKm.Value / 111.0);
                double lonMin = longitud.Value - (radioKm.Value / 111.0);
                double lonMax = longitud.Value + (radioKm.Value / 111.0);

                filter &= Builders<PuntoGeo>.Filter.And(
                    Builders<PuntoGeo>.Filter.Gte(p => p.Latitud, latMin),
                    Builders<PuntoGeo>.Filter.Lte(p => p.Latitud, latMax),
                    Builders<PuntoGeo>.Filter.Gte(p => p.Longitud, lonMin),
                    Builders<PuntoGeo>.Filter.Lte(p => p.Longitud, lonMax)
                );
            }

            return await _puntos.Find(filter).ToListAsync();
        }

        public async Task<PuntoGeo?> ObtenerPuntoGeoPorId(string id)
        {
            return await _puntos.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<PuntoGeo> CrearPuntoGeo(PuntoGeo puntoGeo)
        {
            puntoGeo.FechaCreacion = DateTime.UtcNow;
            await _puntos.InsertOneAsync(puntoGeo);
            return puntoGeo;
        }

        public async Task<bool> ActualizarPuntoGeo(string id, PuntoGeo puntoGeo, string usuarioId)
        {
            var filtro = Builders<PuntoGeo>.Filter.And(
                Builders<PuntoGeo>.Filter.Eq(p => p.Id, id),
                Builders<PuntoGeo>.Filter.Eq(p => p.UsuarioIngreso, usuarioId)
            );

            var resultado = await _puntos.ReplaceOneAsync(filtro, puntoGeo);
            return resultado.ModifiedCount > 0;
        }

        public async Task<bool> EliminarPuntoGeo(string id, string usuarioId)
        {
            var filtro = Builders<PuntoGeo>.Filter.And(
                Builders<PuntoGeo>.Filter.Eq(p => p.Id, id),
                Builders<PuntoGeo>.Filter.Eq(p => p.UsuarioIngreso, usuarioId)
            );

            var resultado = await _puntos.DeleteOneAsync(filtro);
            return resultado.DeletedCount > 0;
        }
    }

    public interface IDalPuntoGeo
    {
        Task<List<PuntoGeo>> ObtenerPuntoGeo(string? tipo, double? latitud, double? longitud, double? redioKm);
        Task<PuntoGeo?> ObtenerPuntoGeoPorId(string id);
        Task<PuntoGeo> CrearPuntoGeo(PuntoGeo puntoGeo);
        Task<bool> ActualizarPuntoGeo(string id, PuntoGeo puntoGeo, string usuarioId);
        Task<bool> EliminarPuntoGeo(string id, string usuarioId);
    }
}
