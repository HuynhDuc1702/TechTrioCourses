using AutoMapper;
using CategoryAPI.Datas;
using CategoryAPI.DTOs.Request;
using CategoryAPI.DTOs.Response;
using CategoryAPI.Models;
using CategoryAPI.Repositories.Interfaces;
using CategoryAPI.Services.Interfaces;

namespace CategoryAPI.Services
{
    public class CategoryService : ICategoryService

    {
        private readonly ICategoryRepo _coursesRepo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepo coursesRepo, IMapper mapper)
        {
            _coursesRepo = coursesRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategorysAsync()
        {
            var courses = await _coursesRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryResponse>>(courses);
        }

        public async Task<CategoryResponse?> GetCategoryByIdAsync(Guid id)
        {
            var course = await _coursesRepo.GetByIdAsync(id);

            if (course == null)
            {
                return null;
            }

            return _mapper.Map<CategoryResponse>(course);
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategoriesByIdsAsync(List<Guid> ids)
        {
            var categories = await _coursesRepo.GetByIdsAsync(ids);
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var course = _mapper.Map<Category>(request);

            var createdCategory = await _coursesRepo.CreateAsync(course);

            return _mapper.Map<CategoryResponse>(createdCategory);
        }

        public async Task<CategoryResponse?> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request)
        {
            var existingCategory = await _coursesRepo.GetByIdAsync(id);

            if (existingCategory == null)
            {
                return null;
            }

            // Map only non-null properties from request to existing course
            if (request.Name != null)
                existingCategory.Name = request.Name;

            if (request.Description != null)
                existingCategory.Description = request.Description;

            

            var updatedCategory = await _coursesRepo.UpdateAsync(existingCategory);

            if (updatedCategory == null)
            {
                return null;
            }

            return _mapper.Map<CategoryResponse>(updatedCategory);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            return await _coursesRepo.DeleteAsync(id);
        }

    }
}
