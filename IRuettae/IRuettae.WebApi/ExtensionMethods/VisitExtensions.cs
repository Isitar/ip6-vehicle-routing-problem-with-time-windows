using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Persistence.Entities;

namespace IRuettae.WebApi.ExtensionMethods
{
    public static class VisitExtensions
    {
        public static string RouteCalcAddress(this Visit v) => v.Zip == 0 ? $"{v.Street}" : $"{v.Street}, {v.Zip} {v.City}";
    }
}