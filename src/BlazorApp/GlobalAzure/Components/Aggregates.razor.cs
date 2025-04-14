using System.Text.Json;
using GlobalAzure.Events;
using GlobalAzure.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace GlobalAzure.Components;

public class AggregatesBase : ComponentBase, IAsyncDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = null!;
    protected MudTable<ResolvedAggregate> MudTable { get; set; } = new();

    [Parameter]
    public IEnumerable<ResolvedAggregate>? Aggregates { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
    
    protected Task RowClickEvent(TableRowClickEventArgs<ResolvedAggregate> tableRowClickEventArgs)
    {
        return Bus.Publish(new MufloneEvent("AggregateSelected", JsonSerializer.Serialize(tableRowClickEventArgs.Item)));
    }
    
    #region Dispose
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncInternal();
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncInternal()
    {
        // Async cleanup mock
        await Task.Yield();
    }
    #endregion
}