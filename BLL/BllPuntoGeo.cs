using DAL;
using Model;
using MongoDB.Bson;

namespace BLL
{
    public class BllPuntoGeo
    {
        private readonly IDalPuntoGeo _dal;

        public BllPuntoGeo(IDalPuntoGeo dal)
        {
            _dal = dal;
        }

        public async Task<List<PuntoGeo>> ObtenerPuntoGeo(string? tipo, double? latitud, double? longitud, double? redioKm)
        {
            return await _dal.ObtenerPuntoGeo(tipo, latitud, longitud, redioKm);
        }

        public async Task<PuntoGeo?> ObtenerPuntoGeoPorId(string id)
        {
            return await _dal.ObtenerPuntoGeoPorId(id);
        }

        public async Task<PuntoGeo> CrearPuntoGeo(PuntoGeo puntoGeo)
        {
            return await _dal.CrearPuntoGeo(puntoGeo);
        }

        public async Task<bool> ActualizarPuntoGeo(string id, PuntoGeo puntoGeo, string usuarioId)
        {
            return await _dal.ActualizarPuntoGeo(id, puntoGeo, usuarioId);
        }

        public async Task<bool> EliminarPuntoGeo(string id, string usuarioId)
        {
            return await _dal.EliminarPuntoGeo(id, usuarioId);
        }
    }
}
