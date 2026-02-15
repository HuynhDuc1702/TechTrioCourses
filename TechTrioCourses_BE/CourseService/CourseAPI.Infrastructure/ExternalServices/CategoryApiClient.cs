using CourseAPI.Application.DTOs.Response;
using CourseAPI.Application.Interfaces.IExternalServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TechTrioCourses.Shared.Dtos.Category;

namespace CourseAPI.Infrastructure.ExternalServices
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoryApiClient> _logger;
        private readonly IMemoryCache _cache;

        public CategoryApiClient(
            IHttpClientFactory httpClientFactory,
            ILogger<CategoryApiClient> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient("CategoryAPI");
            _logger = logger;
            _cache = cache;
        }

        public async Task<CategoryResponse?> GetCategoryByIdAsync(Guid id)
        {
            string cacheKey = $"Category_{id}";

            if (_cache.TryGetValue(cacheKey, out CategoryResponse? cached))
            {
                return cached;
            }

            try
            {
                var response = await _httpClient.GetFromJsonAsync<CategoryResponse>(
                    $"api/categories/{id}");

                if (response != null)
                {
                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(30));
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch category {CategoryId}", id);
                return null;
            }
        }
        public async Task<Dictionary<Guid, string>> GetCategoryNamesAsync(IEnumerable<Guid> categoryIds)
        {
            var ids = categoryIds.Distinct().ToList();
            if (!ids.Any())
                return new();

            var cacheKey = $"Categories_{string.Join("_", ids.OrderBy(x => x))}";

            if (_cache.TryGetValue(cacheKey, out Dictionary<Guid, string>? cached))
            {
                _logger.LogInformation("Using cached categories");
                return cached!;
            }

            try
            {
                var response = await _httpClient
                    .PostAsJsonAsync("api/categories/get-by-ids", ids);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch categories. Status: {Status}",
                        response.StatusCode);
                    return new();
                }

                var categories = await response.Content
                    .ReadFromJsonAsync<List<CategoryResponse>>()
                    ?? new();

                var result = categories.ToDictionary(c => c.Id, c => c.Name);

                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                return new();
            }
        }

        public async Task PopulateCategoryNameAsync(List<CourseResponse> courses)
        {
            var categoryIds = courses
                .Where(c => c.CategoryId.HasValue)
                .Select(c => c.CategoryId!.Value)
                .Distinct();

            var categoryMap = await GetCategoryNamesAsync(categoryIds);

            foreach (var course in courses)
            {
                if (course.CategoryId.HasValue &&
                    categoryMap.TryGetValue(course.CategoryId.Value, out var name))
                {
                    course.CategoryName = name;
                }
            }
        }


    }
}
