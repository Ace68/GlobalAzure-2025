using System.Text.Json;
using BlazorComponentBus;
using GlobalAzure.Events;
using GlobalAzure.Services;
using Microsoft.AspNetCore.Components;

namespace GlobalAzure.Pages;

public class MufloneBase : ComponentBase, IAsyncDisposable
{
    [Inject] private IAggregateService AggregateService { get; set; } = null!;
    
    [Inject] private ComponentBus Bus { get; set; } = null!;
    
    protected IEnumerable<ResolvedEvent>? Events { get; set; } = [];
    protected IEnumerable<ResolvedAggregate>? Aggregates { get; set; } = [];
    
    protected bool HideAggregate = false;
    protected bool HideEvents = true;
    
    protected override async Task OnInitializedAsync()
    {
        Bus.Subscribe<MufloneEvent>(MessageAddedHandler);
        
        await LoadAggregatesAsync();
        await LoadAggregateStreamAsync();
        
        await base.OnInitializedAsync();
    }

    private async Task LoadAggregatesAsync()
    {
        Aggregates = await AggregateService.GetAggregatesAsync(CancellationToken.None);
    }
    
    private async Task LoadAggregateStreamAsync()
    {
        Events = await AggregateService.GetAggregateStreamByIdAsync("9ce9bf05-ac75-4238-bba0-96ba1847f0d4", 0,
            CancellationToken.None);
    }
    
    private async Task MessageAddedHandler(MessageArgs args, CancellationToken ct)
    {
        var message = args.GetMessage<MufloneEvent>().Message;

        switch (message)
        {
            case "AggregateSelected":
                var resolvedAggregate = JsonSerializer.Deserialize<ResolvedAggregate>(args.GetMessage<MufloneEvent>().Body);
                if (resolvedAggregate != null)
                {
                    Events = await AggregateService.GetAggregateStreamByIdAsync(resolvedAggregate.AggregateId, 0,
                        CancellationToken.None);
                    
                    HideAggregate = true;
                    HideEvents = false;
                }
                break;
            
            case "EventSelected":
                HideAggregate = false;
                HideEvents = true;
                break;
        }

        StateHasChanged();
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