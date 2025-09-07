using Blazored.Toast.Services;
using Kaopiz.Shared.Contracts;
using Microsoft.AspNetCore.Components;

namespace Kaopiz.Web.Blazorwasm.Pages.Error
{
    public partial class ErrorElement
    {
        [Inject] private IToastService _toastService { get; set; } = default!;
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;

        [Parameter]
        public List<ErrorDetailDto> Errors { get; set; } = new List<ErrorDetailDto>();

        [Parameter]
        public string FieldName { get; set; } = string.Empty;

        [Parameter]
        public string FormSummaryId { get; set; } = string.Empty;

        private string ErrorMessage { get; set; } = string.Empty;
        private List<string> FormSummaryErrors { get; set; } = new List<string>();

        protected override async Task OnParametersSetAsync()
        {
            await HandleErrorsAsync();
        }

        private async Task HandleErrorsAsync()
        {
            ErrorMessage = string.Empty;
            FormSummaryErrors.Clear();

            if (Errors == null || !Errors.Any()) return;

            var fieldErrors = Errors.Where(e => e.ErrorScope == CErrorScope.Field && $"{FieldName}_Error" == e.Field).Select(e => e.Error).ToList();
            if (!fieldErrors.IsNullOrEmpty())
            {
                ErrorMessage = string.Join("\n", fieldErrors);
            }

            var otherErrors = Errors.Where(s => s.ErrorScope != CErrorScope.None && s.ErrorScope != CErrorScope.FormSummary
                && s.ErrorScope != CErrorScope.Field).ToList();
            if (!otherErrors.IsNullOrEmpty())
            {
                var pageErrors = otherErrors.Where(s => s.ErrorScope == CErrorScope.PageSumarry
                    || s.ErrorScope == CErrorScope.Global).Select(s => s.Error).ToList();
                if (!pageErrors.IsNullOrEmpty())
                {
                    _toastService.ShowError(string.Join(",", pageErrors));
                }
                var redirectErrors = otherErrors.Where(s => s.ErrorScope == CErrorScope.RedirectPage).Select(s => s.Error).ToList();
                var redirectToLoginPage = otherErrors.Where(s => s.ErrorScope == CErrorScope.RedirectToLoginPage).ToList();
                if (!redirectErrors.IsNullOrEmpty())
                {
                    var returnUrl = Uri.EscapeDataString(_navigationManager.Uri);
                    _navigationManager.NavigateTo(uri: $"/login?returnUrl={returnUrl}");
                }
            }

            var formSummaryErrors = Errors.Where(e => e.ErrorScope == CErrorScope.FormSummary).Select(e => e.Error).ToList();

            if (formSummaryErrors.Any())
            {
                FormSummaryErrors.AddRange(formSummaryErrors);
            }

            await InvokeAsync(StateHasChanged);

            await Task.Delay(15000);

            ErrorMessage = string.Empty;
            FormSummaryErrors.Clear();
            Errors.Clear();

            await InvokeAsync(StateHasChanged);
        }
    }
}