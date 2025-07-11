using System;
using System.Collections.Generic;

namespace AdasIt.Andor.Domain.ValuesObjects;

public sealed class DomainResult
{
    public bool IsSuccess => _errors.Count == 0;
    public bool IsFailure => !IsSuccess;

    private readonly ICollection<Notification> _errors;
    public IReadOnlyCollection<Notification> Errors => [.. _errors];

    private readonly ICollection<Notification> _warnings;
    public IReadOnlyCollection<Notification> Warnings => [.. _warnings];

    private readonly ICollection<Notification> _infos;
    public IReadOnlyCollection<Notification> Infos => [.. _infos];

    private DomainResult(ICollection<Notification>? errors,
        ICollection<Notification>? warnings,
        ICollection<Notification>? infos)
    {
        _warnings = warnings ?? [];
        _errors = errors ?? [];
        _infos = infos ?? [];
    }

    public static DomainResult Success(
        ICollection<Notification>? warnings = null,
        ICollection<Notification>? infos = null) => new(null, warnings, infos);

    public static DomainResult Failure(
    ICollection<Notification> errors,
    ICollection<Notification>? warnings = null,
    ICollection<Notification>? infos = null)
    {
        if (errors == null || errors.Count == 0)
        {
            throw new ArgumentException("Failure must contain at least one error notification.");
        }

        return new(errors, warnings, infos);
    }
}
