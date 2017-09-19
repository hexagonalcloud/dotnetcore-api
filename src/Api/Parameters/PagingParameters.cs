using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Api.Parameters
{
    public class PagingParameters
    {
        protected const int MaxPageSize = 20;
        protected const int DefaultPageSize = 10;
        private int _pageNumber;
        private int _pageSize;

        public PagingParameters()
        {
            PageNumber = 1;
            PageSize = DefaultPageSize;
        }

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value > 0 ? value : 1;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= MaxPageSize && value > 0 ? value : DefaultPageSize;
        }

        public ExpandoObject GetParameters()
        {
            Type parameters = GetType();
            PropertyInfo[] properties = parameters.GetProperties();
            var expando = new ExpandoObject();
            foreach (PropertyInfo parameter in properties)
            {
                object value = parameter.GetValue(this, new object[] { });
                expando.TryAdd(parameter.Name, value);
            }

            return expando;
        }
    }
}
