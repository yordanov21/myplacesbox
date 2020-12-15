namespace MyPlacesBox.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MyPlacesBox.Data;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;

    [Area("Administration")]
    public class RegionsController : AdministrationController
    {
        private readonly IDeletableEntityRepository<Region> dataRepository;

        public RegionsController(IDeletableEntityRepository<Region> dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        // GET: Administration/Regions
        public async Task<IActionResult> Index()
        {
            return this.View(await this.dataRepository.All().ToListAsync());
        }

        // GET: Administration/Regions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var region = await this.dataRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (region == null)
            {
                return this.NotFound();
            }

            return this.View(region);
        }

        // GET: Administration/Regions/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Regions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Region region)
        {
            if (this.ModelState.IsValid)
            {
                await this.dataRepository.AddAsync(region);
                await this.dataRepository.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(region);
        }

        // GET: Administration/Regions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var region = this.dataRepository.All().FirstOrDefault( x=>x.Id == id);
            if (region == null)
            {
                return this.NotFound();
            }

            return this.View(region);
        }

        // POST: Administration/Regions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Region region)
        {
            if (id != region.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.dataRepository.Update(region);
                    await this.dataRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.RegionExists(region.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(region);
        }

        // GET: Administration/Regions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var region = await this.dataRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (region == null)
            {
                return this.NotFound();
            }

            return this.View(region);
        }

        // POST: Administration/Regions/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var region = this.dataRepository.All().FirstOrDefault(x => x.Id == id);
            this.dataRepository.Delete(region);
            await this.dataRepository.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool RegionExists(int id)
        {
            return this.dataRepository.All().Any(e => e.Id == id);
        }
    }
}
