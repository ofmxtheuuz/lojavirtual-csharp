using System.Globalization;
using LojaVirtual.Data;
using LojaVirtual.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Services;

public class pedidoService
{
    private readonly AppDbContext _context;

    public pedidoService(AppDbContext context)
    {
        _context = context;
    }  

    public async Task<string> AddPedido(Pedido pedido, string token)
    {
        var ped = new Pedido()
        {
            Nome = pedido.Nome,
            Sobrenome = pedido.Sobrenome,
            Endereco = pedido.Endereco,
            Cep = pedido.Cep,
            Estado = pedido.Estado,
            Cidade = pedido.Cidade,
            Email = pedido.Email,
            PedidoTotal = pedido.PedidoTotal,
            TotalItensPedido = pedido.TotalItensPedido,
            PedidoEnviado = pedido.PedidoEnviado,
            token = token,

        };
        await _context.Pedidos.AddAsync(ped);
        return ped.token;
    }

    public async Task AddPedidoDetalhe(PedidoDetalhe pedidodetails)
    {
        var pedidod = new PedidoDetalhe()
        {
            Pedido = pedidodetails.Pedido,
            Lanche = pedidodetails.Lanche,
            Quantidade = pedidodetails.Quantidade,
            Preco = pedidodetails.Preco
        };
        await _context.PedidoDetalhes.AddAsync(pedidodetails);
    }

    public async Task<string> AddFatura(Fatura fatura, string token)
    {
        Random randNum = new Random();
        var num = randNum.Next();
        var pedido = await _context.Pedidos.FirstOrDefaultAsync(ped => ped.token == token);
        var external = num + "-PAYMENT";
        var fat = new Fatura()
        {
            FaturaId = Guid.NewGuid().ToString(),
            Pedido = pedido,
            FaturaStatus = "pending",
            ExternalReference = external,
            Preco = fatura.Preco,
            CreatedDate = DateTime.Now.ToString(),
            IPAddress = "::0",
            PreferenceMPId = "0000000",
            MetodoDePagamento = "payment",
            LastUpdatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
        };

        await _context.Faturas.AddAsync(fat);
        return external;
    }

}