using BancoClientes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BancoClientes.Controllers
{
    public class ClientesController : Controller
    {
        private readonly Contexto _contexto;
        public ClientesController(Contexto contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            List<Cliente> lstCliente = _contexto.Cliente.ToList();
            CarregarSexoCliente();
            return View(lstCliente);
        }

        public IActionResult IndexComp()
        {
            List<Cliente> lstCliente = _contexto.Cliente.ToList();
            CarregarSexoCliente();
            return View(lstCliente);
        }

        #region CRUD

        [HttpGet]
        public IActionResult Create()
        {
            Cliente cli = new Cliente();
            CarregarSexoCliente();
            return View(cli);
        }

        [HttpPost]
        public IActionResult Create(Cliente pCli)
        {
            try
            {
                ValidarCampos(pCli);
                pCli.UsuarioCadastro = string.Empty;
                pCli.DataCadastro = DateTime.Now;

                _contexto.Cliente.Add(pCli);
                _contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MsgErro = ex.Message;
                CarregarSexoCliente();
                return View(pCli);
            }
        }

        [HttpGet]
        public IActionResult Edit(int pId)
        {
            Cliente cli = _contexto.Cliente.Find(pId);
            if (cli != null)
            {
                CarregarSexoCliente();
                return View(cli);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Cliente pCli)
        {
            try
            {
                ValidarCampos(pCli);
                pCli.UsuarioAtualizacao = string.Empty;
                pCli.DataAtualizacao = DateTime.Now;

                _contexto.Cliente.Update(pCli);
                _contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MsgErro = ex.Message;
                CarregarSexoCliente();
                return View(pCli);
            }
        }

        [HttpGet]
        public IActionResult Details(int pId)
        {
            try
            {
                Cliente cli = _contexto.Cliente.Find(pId);
                if (cli != null)
                {
                    CarregarSexoCliente();
                    return View(cli);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MsgErro = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Delete(int pId)
        {
            Cliente cli = _contexto.Cliente.Find(pId);
            if (cli != null)
            {
                CarregarSexoCliente();
                return View(cli);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Cliente pCli)
        {
            try
            {
                _contexto.Cliente.Remove(pCli);
                _contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MsgErro = ex.Message;
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region Métodos Privados

        private void ValidarCampos(Cliente pCli)
        {
            StringBuilder sbErro = new StringBuilder();
            if (string.IsNullOrWhiteSpace(pCli.Nome))
                sbErro.AppendLine("* Nome de cliente não preenchido corretamente");

            if (string.IsNullOrWhiteSpace(pCli.Cpf))
                sbErro.AppendLine("* CPF do cliente não preenchido corretamente");
            else if (!ValidarCpf(pCli.Cpf))
                sbErro.AppendLine("* CPF do cliente não é válido.");
            else if ((_contexto.Cliente.First(cli => cli.Cpf.Equals(pCli.Cpf))) != null)
                sbErro.AppendLine("* Já existe um cliente cadastrado para o CPF informado.");

            if (pCli.DataNascimento == DateTime.MinValue)
                sbErro.AppendLine("* Data de Nascimento do cliente não preenchida corretamente");

            string padraoEmail = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            if (string.IsNullOrWhiteSpace(pCli.Email))
                sbErro.AppendLine("* E-Mail do cliente não preenchido corretamente");
            else if (Regex.IsMatch(pCli.Email, padraoEmail))
                sbErro.AppendLine("* E-Mail do cliente não é válido.");

            if (!string.IsNullOrWhiteSpace(sbErro.ToString()))
                throw new Exception("Foram encontrados os seguintes erros:" + Environment.NewLine + sbErro.ToString());
        }

        private bool ValidarCpf(string pCpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };


            pCpf = pCpf.Trim();
            pCpf = pCpf.Replace(".", "").Replace("-", "");
            if (pCpf.Length != 11)
                return false;

            string tempCpf = pCpf.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();
            return pCpf.EndsWith(digito);
        }

        private void CarregarSexoCliente()
        {
            List<SelectListItem> tiposSexoCli = new List<SelectListItem>()
            {
                new SelectListItem{Value = "M", Text="Masculino"},
                new SelectListItem{Value = "F", Text="Feminino"},
                new SelectListItem{Value = "O", Text="Outros"}
            };

            ViewBag.tiposSexoCli = tiposSexoCli;
        }

        #endregion
    }
}
