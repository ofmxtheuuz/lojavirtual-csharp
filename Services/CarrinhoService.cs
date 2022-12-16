using LojaVirtual.Data;

namespace LojaVirtual.Services;

public class CarrinhoService
{

    private readonly AppDbContext _appDbContext;

    public CarrinhoService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task LimparCarrinho(string carrinhoId)
    {
        var itens = _appDbContext.CarrinhoCompraItems.Where(x => x.CarrinhoCompraId == carrinhoId).AsEnumerable();
        _appDbContext.CarrinhoCompraItems.RemoveRange(itens);
        await _appDbContext.SaveChangesAsync();
    }
}