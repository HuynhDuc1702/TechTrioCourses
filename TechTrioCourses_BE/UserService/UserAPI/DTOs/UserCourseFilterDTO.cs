using System;
using TechTrioCourses.Shared.Enums;
using TechTrioCourses.Shared.Filters;

namespace UserAPI.DTOs
{
    public class UserCourseFilterDTO : BaseFilterDTO
    {
        [FilterField(FilterOperationEnum.Equal, "Status")]
        public UserCourseStatusEnum? Status { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, "Progress")]
        public double? MinProgress { get; set; }

        [FilterField(FilterOperationEnum.Equal, "CourseId")]
        public Guid? CourseId { get; set; }
    }
}
