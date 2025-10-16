using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperWrapper.Attributes
{
    public enum KeyGeneration
    {
        None,           // Not a key
        AutoIncrement,  // Let the DB generate it (identity)
        GenerateGuid,   // Generate a GUID in code
        External        // Provided by the model (string, GUID, etc.)
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnAttribute : Attribute
    {
        public string? Name { get; set; }
        public bool IsPrimaryKey { get; set; }
        public KeyGeneration KeyGeneration { get; set; } = KeyGeneration.None;
    }

}
