using Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL
{
    public class DalUsuario : IDalUsuario
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public DalUsuario(IMongoDatabase database)
        {
            _usuarios = database.GetCollection<Usuario>("Usuarios");
        }

        public async Task<Usuario?> ObtenerUsuarioPorId(string id)
        {
            return await _usuarios.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Usuario?> ObtenerUsuarioPorUsername(string username)
        {
            return await _usuarios.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
            await _usuarios.InsertOneAsync(usuario);
            return usuario;
        }

        public async Task<bool> EliminarUsuario(string id)
        {
            var result = await _usuarios.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ActualizarUsuario(string id, Usuario usuario)
        {
            var result = await _usuarios.ReplaceOneAsync(u => u.Id == id, usuario);
            return result.ModifiedCount > 0;
        }

    }

    public interface IDalUsuario
    {
        Task<Usuario?> ObtenerUsuarioPorUsername(string username);
        Task<Usuario?> ObtenerUsuarioPorId(string id);
        Task<Usuario> CrearUsuario(Usuario usuario);
        Task<bool> EliminarUsuario(string id);
        Task<bool> ActualizarUsuario(string id, Usuario usuario);
    }
}
