using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.ReadModel.Dtos
{
    public class LastEventPosition : EntityBase
	{
		public ulong CommitPosition { get; set; }
		public ulong PreparePosition { get; set; }
	}
}
