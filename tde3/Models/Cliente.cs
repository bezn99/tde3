using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace tde3.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [PrimaryKey, AutoIncrement]
        public int CodigoCliente { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; }

        public int RG { get; set; }

        [MaxLength(14)]
        public string Telefone { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Compra> Compras { get; set; }
    }
}