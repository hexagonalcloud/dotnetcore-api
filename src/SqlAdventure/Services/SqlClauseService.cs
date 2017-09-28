using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlAdventure.Services
{
    public class SqlClauseService : ISqlClauseService
    {
        private readonly string[] _allowedOrderParameters = { "Color", "Color desc", "Color asc", "Name", "Name asc", "Name desc" };
        private readonly string _defaultOrder = "Name";

        public (string predicate, object[] parameters) CreateWhereClause(string columnName, string parameter)
        {
            string predicate = string.Empty;
            List<object> sqlParams = new List<object>();

            if (String.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            if (String.IsNullOrWhiteSpace(parameter))
            {
                return (predicate, sqlParams.ToArray());
            }

            var list = ParameterToList(parameter);
            int i = 0;

            foreach (var color in list)
            {
                if (i == 0)
                {
                    predicate = $"{columnName}==@{i}";
                    sqlParams.Add(color);
                }
                else
                {
                    predicate += $" OR {columnName}==@{i}";
                    sqlParams.Add(color);
                }

                i++;
            }

            return (predicate, sqlParams.ToArray());
        }

        public string CreateOrderClause(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return _defaultOrder;
            }

            var orderClause = String.Empty;
            var list = ParameterToList(parameter);
            if (String.IsNullOrWhiteSpace(parameter))
            {
                return String.Empty;
            }

            int i = 0;

            foreach (var item in list)
            {
                if(_allowedOrderParameters.Contains(item, StringComparer.OrdinalIgnoreCase))
                {
                    if (i == 0)
                    {
                        orderClause += item;
                    }
                    else
                    {
                        orderClause += ", " + item;
                    }

                    i++;
                }
            }

            if (string.IsNullOrWhiteSpace(orderClause))
            {
                return _defaultOrder;
            }

            return orderClause;
        }

        private static IEnumerable<string> ParameterToList(string parameter)
        {
            var list = parameter.Split(',');
            var trimmedList = new List<string>();
            foreach (var s in list)
            {
               trimmedList.Add(s.Trim());  
            }

            return trimmedList;
        }
    }
}
