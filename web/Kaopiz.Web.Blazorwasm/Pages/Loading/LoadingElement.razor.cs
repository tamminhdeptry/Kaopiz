using Microsoft.AspNetCore.Components;

namespace Kaopiz.Web.Blazorwasm.Pages.Loading
{
    public partial class LoadingElement
    {
        [Parameter] public bool IsLoading { get; set; } = false;
        [Parameter] public EventCallback<bool> IsLoadingChanged { get; set; }
    }
}