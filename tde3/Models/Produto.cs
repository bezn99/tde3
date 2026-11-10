using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace tde3.Models
{
    [Table("Produto")]
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int CodigoProduto { get; set; }

        [MaxLength(50)]
        public string NomeProduto { get; set; }

        [MaxLength(50)]
        public string Tipo { get; set; }

        public decimal Preco { get; set; }

        public int QtdEstoque { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ItemCompra> ItensCompra { get; set; }
    }
}