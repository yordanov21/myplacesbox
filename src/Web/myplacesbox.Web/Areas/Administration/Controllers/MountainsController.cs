namespace MyPlacesBox.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MyPlacesBox.Data;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;

    public class MountainsController : AdministrationController
    {
        private readonly IDeletableEntityRepository<Mountain> dataRepository;

        public MountainsController(IDeletableEntityRepository<Mountain> dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        // GET: Administration/Mountains
        public async Task<IActionResult> Index()
        {
            return this.View(await this.dataRepository.All().ToListAsync());
        }

        // GET: Administration/Mountains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var mountain = await this.dataRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mountain == null)
            {
                return this.NotFound();
            }

            return this.View(mountain);
        }

        // GET: Administration/Mountains/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Mountains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Mountain mountain)
        {
            if (this.ModelState.IsValid)
            {
                await this.dataRepository.AddAsync(mountain);
                await this.dataRepository.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(mountain);
        }

        // GET: Administration/Mountains/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var mountain = this.dataRepository.All().FirstOrDefault(x => x.Id == id);
            if (mountain == null)
            {
                return this.NotFound();
            }

            return this.View(mountain);
        }

        // POST: Administration/Mountains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Mountain mountain)
        {
            if (id != mountain.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.dataRepository.Update(mountain);
                    await this.dataRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.MountainExists(mountain.Id))
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

            return this.View(mountain);
        }

        // GET: Administration/Mountains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var mountain = await this.dataRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mountain == null)
            {
                return this.NotFound();
            }

            return this.View(mountain);
        }

        // POST: Administration/Mountains/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mountain = this.dataRepository.All().FirstOrDefault(x => x.Id == id);
            this.dataRepository.Delete(mountain);
            await this.dataRepository.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool MountainExists(int id)
        {
            return this.dataRepository.All().Any(e => e.Id == id);
        }
    }
}
