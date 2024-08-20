using System.ComponentModel.DataAnnotations;

namespace ControleClientesMvc.Models
{
	public class UsuarioModel
	{
        public int Id { get; set; }
		[Required(ErrorMessage = "O nome é obrigatório.")]
		[MaxLength(50)]
        public string? Nome { get; set; }
        [MaxLength(100)]
		[Required(ErrorMessage = "O e-mail é obrigatório.")]
		[EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
		public string? Email { get; set; }
        [MaxLength(20)]
		[Required(ErrorMessage = "A senha é obrigatória.")]
		public string? Senha { get; set; }
    }
}
