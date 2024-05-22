﻿using MvcCoreElastiCacheAWS.Helpers;
using MvcCoreElastiCacheAWS.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Services
{
    public class ServiceAWSCache
    {
        private IDatabase cache;
        
        public ServiceAWSCache()
        {
            this.cache = HelperCacheRedis.Connection.GetDatabase();
        }

        public async Task<List<Coche>> GetCochesFavoritosAsync()
        {
            string jsonCoches = await this.cache.StringGetAsync("cochesfavoritos");
            if(jsonCoches == null)
            {
                return null;
            }
            else
            {
                List<Coche> coches = JsonConvert.DeserializeObject<List<Coche>>(jsonCoches);
                return coches;
            }
        }

        public async Task AddCocheFavoritoAsync(Coche coche)
        {
            List<Coche> coches = await this.GetCochesFavoritosAsync();
            if(coches == null)
            {
                coches = new List<Coche>();
            }
            coches.Add(coche);
            string jsonCoches = JsonConvert.SerializeObject(coches);
            await this.cache.StringSetAsync("cochesfavoritos", jsonCoches, TimeSpan.FromMinutes(30));
        }

        public async Task DeleteCocheFavoritoAsync(int idcoche)
        {
            List<Coche> coches = await this.GetCochesFavoritosAsync();
            if(coches != null)
            {
                Coche cocheEliminar = coches.FirstOrDefault(x => x.IdCoche == idcoche);
                coches.Remove(cocheEliminar);
                if (coches.Count == 0)
                {
                    await this.cache.KeyDeleteAsync("cochesfavoritos");
                }
                else 
                {
                    string jsonCoches = JsonConvert.SerializeObject(coches);
                    await this.cache.StringSetAsync("cochesfavoritos",
                        jsonCoches, TimeSpan.FromMinutes(30));
                }

            }
        }
    }
}
