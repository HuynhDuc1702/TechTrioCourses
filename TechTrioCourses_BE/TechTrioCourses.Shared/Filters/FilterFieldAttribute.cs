using System;

namespace TechTrioCourses.Shared.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FilterFieldAttribute : Attribute
    {
        public FilterOperationEnum Operation { get; }
        public string TargetField { get; }

        public FilterFieldAttribute(FilterOperationEnum operation, string targetField = null)
        {
            Operation = operation;
            TargetField = targetField;
        }
    }
}
