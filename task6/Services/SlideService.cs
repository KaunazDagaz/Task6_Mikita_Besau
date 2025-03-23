using task6.Models;
using task6.Services.IServices;

namespace task6.Services
{
    public class SlideService : ISlideService
    {

        private readonly ApplicationDbContext context;

        public SlideService(ApplicationDbContext context)
        {
            this.context = context;
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
