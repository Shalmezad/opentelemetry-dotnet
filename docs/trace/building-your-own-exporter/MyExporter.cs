﻿// <copyright file="MyExporter.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Trace;

internal class MyExporter : ActivityExporter
{
    public override Task<ExportResult> ExportAsync(
        IEnumerable<Activity> batch, CancellationToken cancellationToken)
    {
        // Exporter code which can generate further
        // telemetry should do so inside SuppressInstrumentation
        // scope. This suppresses telemetry from
        // exporter's own code to avoid live-loop situation.
        using var scope = SuppressInstrumentationScope.Begin();

        foreach (var activity in batch)
        {
            Console.WriteLine($"{activity.DisplayName}");
        }

        return Task.FromResult(ExportResult.Success);
    }

    public override Task ShutdownAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"MyExporter.ShutdownAsync");
        return Task.CompletedTask;
    }

    protected override void Dispose(bool disposing)
    {
        Console.WriteLine($"MyExporter.Dispose");
    }
}
