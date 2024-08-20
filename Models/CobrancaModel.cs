using System.ComponentModel.DataAnnotations;

namespace ControleClientesMvc.Models
{
	public class CobrancaModel
	{
        public int Id { get; set; }
        [MaxLength(50)]
        public string Nome { get; set; }
        [MaxLength(255)]
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string? _Data { get; set; }
        public bool Status { get; set; }
        public int _Status { get; set; }
        public int Pagos { get; set; }
        public int Abertos { get; set; }
        public int Atrasados { get; set; }
    }
}