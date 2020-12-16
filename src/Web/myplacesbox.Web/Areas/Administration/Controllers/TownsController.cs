namespace MyPlacesBox.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MyPlacesBox.Data;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;

    public class TownsController : AdministrationController
    {
        private readonly IDeletableEntityRepository<Town> dataRepository;

        public TownsController(IDeletableEntityRepository<Town> dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        // GET: Administration/Towns
        public async Task<IActionResult> Index()
        {
            return this.View(await this.dataRepository.All().ToListAsync());
        }

        // GET: Administration/Towns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var town = await this.dataRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (town == null)
            {
                return this.NotFound();
            }

            return this.View(town);
        }

        // GET: Administration/Towns/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Towns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsTown,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Town town)
        {
            if (this.ModelState.IsValid)
            {
                await this.dataRepository.AddAsync(town);
                await this.dataRepository.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(town);
        }

        // GET: Administration/Towns/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var town = this.dataRepository.All().FirstOrDefault(x => x.Id == id);
            if (town == null)
            {
                return this.NotFound();
            }

            return this.View(town);
        }

        // POST: Administration/Towns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsTown,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Town town)
        {
            if (id != town.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.dataRepository.Update(town);
                    await this.dataRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.TownExists(town.Id))
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

            return this.View(town);
        }

        // GET: Administration/Towns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var town = await this.dataRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (town == null)
            {
                return this.NotFound();
            }

            return this.View(town);
        }

        // POST: Administration/Towns/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var town = this.dataRepository.All().FirstOrDefault(x => x.Id == id);
            this.dataRepository.Delete(town);
            await this.dataRepository.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool TownExists(int id)
        {
            return this.dataRepository.All().Any(e => e.Id == id);
        }
    }
}
