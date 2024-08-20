using System.ComponentModel.DataAnnotations;

namespace ControleClientesMvc.Models
{
	public class ClienteModel
	{
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Nome { get; set; }
        [MaxLength(14)]
        public string? Documento { get; set; }
        [MaxLength(15)]
        public string? Telefone { get; set; }
        [MaxLength(150)]
        public string? Endereco { get; set; }
    }
}
