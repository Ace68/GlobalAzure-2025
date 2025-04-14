using System.Text.Json;
using GlobalAzure.Events;
using GlobalAzure.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace GlobalAzure.Components;

public class AggregateEventsBase : ComponentBase, IAsyncDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = null!;
    protected MudTable<ResolvedEvent> MudTable { get; set; } = new();

    [Parameter]
    public IEnumerable<ResolvedEvent>? Events { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
    
    protected Task RowClickEvent(TableRowClickEventArgs<ResolvedEvent> tableRowClickEventArgs)
    {
        return Bus.Publish(new MufloneEvent("EventSelected", JsonSerializer.Serialize(tableRowClickEventArgs.Item)));
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