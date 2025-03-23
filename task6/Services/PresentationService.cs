using Microsoft.EntityFrameworkCore;
using task6.Models;
using task6.Services.IServices;

namespace task6.Services
{
    public class PresentationService : IPresentationService
    {
        private readonly ApplicationDbContext context;

        public PresentationService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Presentation>> GetAllPresentationsAsync()
        {
            return await context.Presentations
                .Include(p => p.Slides)
                .ToListAsync();
        }

        public async Task<Presentation> GetPresentationByIdAsync(Guid id)
        {
            var presentation = await context.Presentations
                .Include(p => p.Slides
                .OrderBy(s => s.Order))
                .FirstOrDefaultAsync(p => p.Id == id);
            if (presentation == null) throw new ArgumentNullException($"Can't find presentation with guid {id}");
            return presentation;
        }

        public async Task<Presentation> CreatePresentationAsync(string title, string creatorNickname)
        {
            var presentation = new Presentation
            {
                Id = Guid.NewGuid(),
                Title = title,
                CreatorNickname = creatorNickname,
                CreatedAt = DateTime.Now,
                Slides = new List<Slide>
                {
                    new Slide
                    {
                        Id = Guid.NewGuid(),
                        Order = 1,
                        Content = "{\"objects\":[]}"
                    }
                }
            };
            context.Presentations.Add(presentation);
            await context.SaveChangesAsync();
            return presentation;
        }

        public async Task<bool> DeletePresentationAsync(Guid id)
        {
            var presentation = context.Presentations.FirstOrDefault(p => p.Id == id);
            if (presentation == null) throw new ArgumentNullException($"Can't find presentation with guid {id}");
            context.Presentations.Remove(presentation);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Slide> AddSlideAsync(Guid presentationId, int order)
        {
            var slide = new Slide
            {
                Id = Guid.NewGuid(),
                Order = order,
                Content = "{\"objects\":[]}",
                PresentationId = presentationId
            };
            context.Slides.Add(slide);
            await context.SaveChangesAsync();
            return slide;
        }

        public async Task<bool> UpdateSlideContentAsync(Guid slideId, string content)
        {
            var slide = context.Slides.FirstOrDefault(s => s.Id == slideId);
            if (slide == null) throw new ArgumentNullException($"Can't find slide with guid {slideId}");
            slide.Content = content;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSlideAsync(Guid slideId)
        {
            var slide = context.Slides.FirstOrDefault(s => s.Id == slideId);
            if (slide == null) throw new ArgumentNullException($"Can't find slide with guid {slideId}");
            context.Slides.Remove(slide);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
