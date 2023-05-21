namespace SPL.WebApp.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Telerik.Web.Spreadsheet;

    public class SplInfoGeneralRansController : Controller
    {
        //private readonly SPLContext _context;
        //public IWebHostEnvironment WebHostEnvironment { get; set; }

        //public SplInfoGeneralRansController(SPLContext context, IWebHostEnvironment webHostEnvironment)
        //{
        //    _context = context;
        //    WebHostEnvironment = webHostEnvironment;
        //}

        public async Task<IActionResult> Index()
        {
            return View();
            //return View(await _context.SplInfoGeneralRans.ToListAsync());
        }

        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var splInfoGeneralRan = await _context.SplInfoGeneralRans
            //    .FirstOrDefaultAsync(m => m.IdRep == id);
            //if (splInfoGeneralRan == null)
            //{
            //    return NotFound();
            //}
            return View();
            //return View(splInfoGeneralRan);
        }

        // GET: SplInfoGeneralRans/Create
        public IActionResult Create()
        {
            ViewBag.Sheets = Workbook.Load(@"wwwroot/Templates/Plantillas-RAN.xlsx");

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdRep,FechaRep,NoSerie,NoPrueba,ClaveIdioma,Cliente,Capacidad,Resultado,NombreArchivo,Archivo,TipoReporte,ClavePrueba,CantMediciones,Comentario,Creadopor,Fechacreacion,Modificadopor,Fechamodificacion")] SplInfoGeneralRan splInfoGeneralRan)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //_context.Add(splInfoGeneralRan);
        //        //await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(splInfoGeneralRan);
        //}

        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var splInfoGeneralRan = await _context.SplInfoGeneralRans.FindAsync(id);
            //if (splInfoGeneralRan == null)
            //{
            //    return NotFound();
            //}
            return View();
            //return View(splInfoGeneralRan);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(decimal id, [Bind("IdRep,FechaRep,NoSerie,NoPrueba,ClaveIdioma,Cliente,Capacidad,Resultado,NombreArchivo,Archivo,TipoReporte,ClavePrueba,CantMediciones,Comentario,Creadopor,Fechacreacion,Modificadopor,Fechamodificacion")] SplInfoGeneralRan splInfoGeneralRan)
        //{
        //    if (id != splInfoGeneralRan.IdRep)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(splInfoGeneralRan);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SplInfoGeneralRanExists(splInfoGeneralRan.IdRep))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(splInfoGeneralRan);
        //}

        //public async Task<IActionResult> Delete(decimal? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var splInfoGeneralRan = await _context.SplInfoGeneralRans
        //        .FirstOrDefaultAsync(m => m.IdRep == id);
        //    if (splInfoGeneralRan == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(splInfoGeneralRan);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(decimal id)
        //{
        //    var splInfoGeneralRan = await _context.SplInfoGeneralRans.FindAsync(id);
        //    _context.SplInfoGeneralRans.Remove(splInfoGeneralRan);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool SplInfoGeneralRanExists(decimal id)
        //{
        //    return _context.SplInfoGeneralRans.Any(e => e.IdRep == id);
        //}
    }
}
