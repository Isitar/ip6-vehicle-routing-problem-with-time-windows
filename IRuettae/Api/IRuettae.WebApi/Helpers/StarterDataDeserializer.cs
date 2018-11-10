using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Core;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.Manual;
using IRuettae.Persistence.Entities;
using Newtonsoft.Json;

namespace IRuettae.WebApi.Helpers
{
    /// <summary>
    /// Class to desiralize StarterData according to the given type
    /// </summary>
    internal static class StarterDataDeserializer
    {
        public static IStarterData Deserialize(AlgorithmType algorithm, string algorithmData)
        {
            switch (algorithm)
            {
                case AlgorithmType.ILP:
                    return JsonConvert.DeserializeObject<ILPStarterData>(algorithmData);
                case AlgorithmType.Manual:
                    return JsonConvert.DeserializeObject<ManualStarterData>(algorithmData);
            }
            return null;
        }
    }
}