using DBLib.Record.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firestore.Firebase;


namespace Firestore
{
    public static class FirestoreServiceExtension
    {
        public static void AddFirestoreService(this IServiceCollection services)
        {
            services.AddSingleton<FirestoreRepository>();

        }
    }
}
