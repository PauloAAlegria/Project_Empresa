using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Empresa.Data;
using Empresa.Models;

namespace Empresa.Controllers
{
    public class ColaboradoresController : Controller
    {
        private readonly EmpresaContext _context;

        public ColaboradoresController(EmpresaContext context)
        {
            _context = context;
        }

        // GET: Colaboradores
        public async Task<IActionResult> Index()
        {
            var empresaContext = _context.Colaborador.Include(c => c.Departamento).Include(c => c.Funcao);
            return View(await empresaContext.ToListAsync());
        }

        // GET: Colaboradores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador
                .Include(c => c.Departamento)
                .Include(c => c.Funcao)
                .FirstOrDefaultAsync(m => m.ColaboradorId == id);
            if (colaborador == null)
            {
                return NotFound();
            }

            return View(colaborador);
        }

        // GET: Colaboradores/Create
        public IActionResult Create()
        {
            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "DepartamentoId", "NomeDepartamento");
            ViewData["FuncaoId"] = new SelectList(_context.Funcao, "FuncaoId", "NomeFuncao");
            return View();
        }

        // POST: Colaboradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColaboradorId,Nome,Genero,DataNascimento,DocumentoIdentificacao,Email,Telefone,DepartamentoId,FuncaoId")] Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colaborador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "DepartamentoId", "NomeDepartamento", colaborador.DepartamentoId);
            ViewData["FuncaoId"] = new SelectList(_context.Funcao, "FuncaoId", "NomeFuncao", colaborador.FuncaoId);
            return View(colaborador);
        }

        // GET: Colaboradores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador.FindAsync(id);
            if (colaborador == null)
            {
                return NotFound();
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "DepartamentoId", "NomeDepartamento", colaborador.DepartamentoId);
            ViewData["FuncaoId"] = new SelectList(_context.Funcao, "FuncaoId", "NomeFuncao", colaborador.FuncaoId);
            return View(colaborador);
        }

        // POST: Colaboradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ColaboradorId,Nome,Genero,DataNascimento,DocumentoIdentificacao,Email,Telefone,DepartamentoId,FuncaoId")] Colaborador colaborador)
        {
            if (id != colaborador.ColaboradorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colaborador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColaboradorExists(colaborador.ColaboradorId))
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
            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "DepartamentoId", "NomeDepartamento", colaborador.DepartamentoId);
            ViewData["FuncaoId"] = new SelectList(_context.Funcao, "FuncaoId", "NomeFuncao", colaborador.FuncaoId);
            return View(colaborador);
        }

        // GET: Colaboradores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador
                .Include(c => c.Departamento)
                .Include(c => c.Funcao)
                .FirstOrDefaultAsync(m => m.ColaboradorId == id);
            if (colaborador == null)
            {
                return NotFound();
            }

            return View(colaborador);
        }

        // POST: Colaboradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colaborador = await _context.Colaborador.FindAsync(id);
            if (colaborador != null)
            {
                _context.Colaborador.Remove(colaborador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColaboradorExists(int id)
        {
            return _context.Colaborador.Any(e => e.ColaboradorId == id);
        }
    }
}
