﻿using System.Linq.Expressions;
using SqlEventSourcing.Shared.Entities;

namespace SqlEventSourcing.Shared.ReadModel;

public interface IQueries<T> where T : EntityBase
{
	Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);
	Task<PagedResult<T>> GetByFilterAsync(Expression<Func<T, bool>>? query, int page, int pageSize, CancellationToken cancellationToken);
}