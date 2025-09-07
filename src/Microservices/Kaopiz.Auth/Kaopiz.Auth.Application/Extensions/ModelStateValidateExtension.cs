using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Application
{
    public static class ModelStateValidateExtension
    {
        public static CustomModelState ModelStateValidate(this object model)
        {
            var customModelState = new CustomModelState();

            if (model == null)
            {
                customModelState.AddError(field: string.Empty,
                    errorMessage: "Request object is null",
                    errorScope: CErrorScope.FormSummary);
                return customModelState;
            }

            ValidateObject(model, customModelState, string.Empty);
            return customModelState;
        }

        private static void ValidateObject(object model, CustomModelState state, string parentPath)
        {
            if (model == null) return;

            var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetIndexParameters().Length == 0);

            foreach (var property in properties)
            {
                var value = property.GetValue(model);
                var propertyPath = string.IsNullOrEmpty(parentPath) ? property.Name : $"{parentPath}.{property.Name}";

                // Áp dụng các ValidationAttribute
                var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();
                foreach (var attribute in validationAttributes)
                {
                    attribute.ErrorMessage = attribute.ErrorMessage;
                    var errorMessage = attribute.FormatErrorMessage(property.Name);
                    if (!attribute.IsValid(value))
                    {
                        state.AddError(propertyPath, errorMessage);
                    }
                }

                // Nếu là danh sách, validate từng phần tử
                if (value is IEnumerable enumerable && !(value is string))
                {
                    int index = 0;
                    foreach (var item in enumerable)
                    {
                        var indexedPath = $"{propertyPath}[{index}]";
                        ValidateObject(item, state, indexedPath);
                        index++;
                    }
                }
                // Nếu là object phức tạp, validate đệ quy
                else if (value != null && IsComplexType(property.PropertyType))
                {
                    ValidateObject(value, state, propertyPath);
                }
            }
        }

        private static bool IsComplexType(Type type)
        {
            return !(type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal) || type == typeof(Guid) || type == typeof(DateTime));
        }

        public class CustomModelState
        {
            private readonly List<ErrorDetailDto> _errors = new List<ErrorDetailDto>();

            public void AddError(string field, string errorMessage, CErrorScope errorScope = CErrorScope.Field)
            {
                field = string.IsNullOrEmpty(field) ? string.Empty : $"{field}_Error";
                _errors.Add(new ErrorDetailDto
                {
                    ErrorScope = errorScope,
                    Field = field,
                    Error = errorMessage
                });
            }

            public bool IsValid()
            {
                return !_errors.Any();
            }

            public List<ErrorDetailDto> GetErrors()
            {
                return _errors;
            }
        }
    }
}