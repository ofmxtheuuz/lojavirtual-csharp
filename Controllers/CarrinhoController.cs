using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Data;
using LojaVirtual.Models;
using LojaVirtual.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Controllers
{
    [Authorize]
    public class CarrinhoController : Controller
    {
        // Properties
        private readonly AppDbContext _context;

        // Constructor
        public CarrinhoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/carrinho/adicionar/{id:int}")]
        public async Task<IActionResult> AdicionarItemAoCarrinho(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
            var carrinhoid = User.Claims.FirstOrDefault(x => x.Type == "USER_CARRINHO_ID").Value;
            var verification  =await _context.CarrinhoCompraItems.FirstOrDefaultAsync(verify => verify.ProdutoId == id && verify.CarrinhoCompraId == carrinhoid);
            if (verification == null)
            {
                await _context.CarrinhoCompraItems.AddAsync(new ()
                {
                    CarrinhoCompraId = carrinhoid,
                    Produto = produto,
                    Quantity = 1
                });
            }
            else
            {
                verification.Quantity++;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/carrinho/remover/{id:int}")]
        public async Task<IActionResult> RemoverItemDoCarrinho(int id)
        {
            var carrinhoId = User.Claims.FirstOrDefault(x => x.Type == "USER_CARRINHO_ID").Value;
            var item = _context.CarrinhoCompraItems.FirstOrDefault(item => item.ProdutoId == id && item.CarrinhoCompraId == carrinhoId);
            _context.CarrinhoCompraItems.Remove(item);

            _context.SaveChanges();
            return RedirectToAction("FinalizarCarrinho", "Carrinho");
        }
        
        [HttpGet]
        [Route("/carrinho/removerunidade/{id:int}")]
        public async Task<IActionResult> RemoverUnidadeDoCarrinho(int id)
        {
            var carrinhoId = User.Claims.FirstOrDefault(x => x.Type == "USER_CARRINHO_ID").Value;
            var item = _context.CarrinhoCompraItems.FirstOrDefault(item => item.ProdutoId == id && item.CarrinhoCompraId == carrinhoId);
            if (item.Quantity > 1)
            {
                item.Quantity--;
            } else if (item.Quantity == 1)
            {
                _context.CarrinhoCompraItems.Remove(item);
            } 
            _context.SaveChanges();
            return RedirectToAction("FinalizarCarrinho", "Carrinho");
        }

        [HttpGet]
        [Route("/carrinho/finalizar")]
        public async Task<IActionResult> FinalizarCarrinho()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "USER_CARRINHO_ID").Value;
            var carrinho = await _context.CarrinhoCompraItems.Include(x => x.Produto).Where(task => task.CarrinhoCompraId == id).ToListAsync();
            var precototal = 0.00;

            foreach (var item in carrinho)
            {
                precototal += item.Produto.Preco * item.Quantity;
            }
        
            return View(new CheckoutViewModel()
            {
                Carrinho = carrinho,
                QuantidadeProduto = carrinho.Count,
                PrecoTotal = precototal
            });
        }
    }
}