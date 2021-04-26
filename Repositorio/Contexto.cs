using EscudoNarrador.Entidade;
using MongoDB.Bson.Serialization;
using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Tarrasque.Abstracoes;
using System.Security.Authentication;

namespace EscudoNarrador.Repositorio
{
    public class Contexto : MongoContextoBase
    {
        public Contexto(IDbConfiguracao mongoConfig) : base(mongoConfig)
        {
        }

        public override SslProtocols? ProcoloSsl => SslProtocols.Tls12;
        public override bool UsarMapeamentoBsonClassMap => true;
        public override void ConfigurarMapeamentoBsonClassMap()
        {
            BsonClassMap.RegisterClassMap<Sistema>();
            BsonClassMap.RegisterClassMap<Termo>();
        }
    }
}
