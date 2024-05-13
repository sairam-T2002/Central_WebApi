using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Core
{
    public class CoreClass
    {
        public CoreClass( IServiceProvider serviceProvider ) { }
        public void Dispose()
        {
            // Release any resources your service holds (e.g., closing connections)
        }
    }
    public class DBUtils
    {
        public static string filePath = "D:\\Sairam\\Solutions\\ConsoleApp\\WebApioneService\\appsettings.json";
        public static string GetConnectionString()
        {
            string jsonString = File.ReadAllText(filePath);
            dynamic? jsonObject = JsonConvert.DeserializeObject(jsonString);
            string? connection_str = jsonObject?.ConnectionStrings.DefaultConnection;
            return connection_str;
        }
    }
    public class DBParameters : List<DBParameter>
    {
        public void Add( string key )
        {
            Add(new DBParameter(key));
        }

        public void Add( string key, object value )
        {
            Add(new DBParameter(key, value));
        }

        public void Add( string key, string value, string? defaultValue = null, bool isFunction = false )
        {
            Add(new DBParameter(key, value, defaultValue, isFunction));
        }
    }
    public class DBParameter
    {
        private object _value;

        public bool IsFunction { get; private set; }

        public string Key { get; set; }

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public DBParameter()
        {
        }

        public DBParameter( string key )
        {
            Key = key;
            Value = DBNull.Value;
        }

        public DBParameter( string key, object value )
        {
            Key = key;
            Value = value ?? DBNull.Value;
        }

        public DBParameter( string key, string value, string? defaultValue = null, bool isFunction = false )
        {
            Key = key;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (defaultValue == null)
                {
                    Value = DBNull.Value;
                }
                else
                {
                    Value = defaultValue;
                }
            }
            else
            {
                Value = value;
            }

            IsFunction = isFunction;
        }
    }
}
