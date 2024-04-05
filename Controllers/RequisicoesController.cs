using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Empresa.Data;
using Empresa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Empresa.Controllers
{
    public class RequisicoesController : Controller
    {
        private readonly EmpresaContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RequisicoesController(EmpresaContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Requisicoes
        public async Task<IActionResult> Index()
        {
            var empresaContext = _context.Requisicao.Include(r => r.Produto).Include(r => r.User);
            return View(await empresaContext.ToListAsync());
        }

        // GET: Requisicoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisicao = await _context.Requisicao
                .Include(r => r.Produto)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RequisicaoId == id);
            if (requisicao == null)
            {
                return NotFound();
            }

            return View(requisicao);
        }

        // GET: Requisicoes/Create
        public IActionResult Create()
        {
            Requisicao requisicao = new Requisicao();
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "Nome");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            requisicao.Data = DateTime.Now;
            return View(requisicao);
        }

        // POST: Requisicoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequisicaoId,UserId,ProdutoId,Quantidade,Data,Pedido,Status")] Requisicao requisicao)
        {
            if (ModelState.IsValid)
            {
                //obter o user que está logado no momento da criação da requisição
                var user = await _userManager.GetUserAsync(User);
                requisicao.User = user;

                _context.Add(requisicao);                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "Nome", requisicao.ProdutoId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", requisicao.UserId);
            return View(requisicao);
        }


        [Authorize(Roles = "SupAdmin")]
        // GET: Requisicoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisicao = await _context.Requisicao.FindAsync(id);
            if (requisicao == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            requisicao.User = user;

            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "Nome", requisicao.ProdutoId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", requisicao.UserId);
            return View(requisicao);
        }


        [Authorize(Roles = "SupAdmin")]
        // POST: Requisicoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequisicaoId,UserId,ProdutoId,Quantidade,Data,Pedido,Status")] Requisicao requisicao, Produto Produto)
        {
            if (id != requisicao.RequisicaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // caso o status da requisição for aprovado, irá subtrair
                    // a quantidade do produto requisitado no total dos produtos
                    if (requisicao.Status == Status.Aprovado)
                    {
                        var produto = _context.Produto.Find(Produto.ProdutoId);
                        produto.Quantidade = produto.Quantidade - requisicao.Quantidade;
                        _context.Update(produto);
                        _context.SaveChanges();

                    }
                    _context.Update(requisicao);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequisicaoExists(requisicao.RequisicaoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "Nome", requisicao.ProdutoId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", requisicao.UserId);
            return View(requisicao);
        }

        // GET: Requisicoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisicao = await _context.Requisicao
                .Include(r => r.Produto)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RequisicaoId == id);
            if (requisicao == null)
            {
                return NotFound();
            }

            return View(requisicao);
        }

        // POST: Requisicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requisicao = await _context.Requisicao.FindAsync(id);
            if (requisicao != null)
            {
                _context.Requisicao.Remove(requisicao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequisicaoExists(int id)
        {
            return _context.Requisicao.Any(e => e.RequisicaoId == id);
        }
    }
}
