using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace tde3.Models
{
    [Table("Compra")]
    public class Compra
    {
        [PrimaryKey, AutoIncrement]
        public int CodigoCompra { get; set; }

        public DateTime DataCompra { get; set; }

        public decimal ValorTotal { get; set; }

        [ForeignKey(typeof(Cliente))]
        public int CodigoCliente { get; set; }

        [ManyToOne]
        public Cliente Cliente { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ItemCompra> ItensCompra { get; set; }
    }
}