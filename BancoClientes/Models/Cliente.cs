using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BancoClientes.Models
{
    [Table("TBCLI")]
    public class Cliente
    {
        [Display(Description = "Código")]
        public int Id { get; set; }
        [Display(Description = "Nome")]
        public string Nome { get; set; }
        [Display(Description = "CPF")]
        public string Cpf { get; set; }
        [Display(Description = "Sexo")]
        public string Sexo { get; set; }
        [Display(Description = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }
        [Display(Description = "Nacionalidade")]
        public string Nacionalidade { get; set; }
        [Display(Description = "Natural de")]
        public string Naturalidade { get; set; }
        [Display(Description = "E-Mail")]
        public string Email { get; set; }
        [Display(Description = "Logradouro")]
        public string Logradouro { get; set; }
        [Display(Description = "Numero")]
        public int Numero { get; set; }
        [Display(Description = "Complemento")]
        public string Complemento { get; set; }
        [Display(Description = "Bairro")]
        public string Bairro { get; set; }
        [Display(Description = "Cidade")]
        public string Cidade { get; set; }
        [Display(Description = "Estado")]
        public string Estado { get; set; }
        [Display(Description = "Pais")]
        public string Pais { get; set; }
        [Display(Description = "Cep")]
        public string Cep { get; set; }
        [Display(Description = "Usuário de Cadastro")]
        public string UsuarioCadastro { get; set; }
        [Display(Description = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }
        [Display(Description = "Usuário da Última Atualização")]
        public string UsuarioAtualizacao { get; set; }
        [Display(Description = "Data da Última Atualização")]
        public DateTime DataAtualizacao { get; set; }
    }
}
